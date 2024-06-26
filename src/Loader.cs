using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using HarmonyLib;

namespace FontPatcher;

[HarmonyPatch]
class FontLoader
{
    class FontBundle
    {
        public string BundleName;
        public TMP_FontAsset Normal;
        public TMP_FontAsset Transmit;
    }

    static List<FontBundle> fontBundles = new();
    static Regex normalRegex;
    static Regex transmitRegex;

    public static void Load()
    {
        try
        {
            string configPath = Path.GetDirectoryName(Plugin.Instance.Config.ConfigFilePath);
            string fontsPath = Path.Combine(configPath, Plugin.configFontAssetPath.Value);
            Plugin.LogInfo($"Font path: {fontsPath}");

            DirectoryInfo di = new DirectoryInfo(fontsPath);
            FileInfo[] fileInfos = di.GetFiles("*");

            int sucessCount = 0;
            int failCount = 0;
            foreach (FileInfo info in fileInfos)
            {
                try
                {
                    AssetBundle bundle = AssetBundle.LoadFromFile(info.FullName);
                    Plugin.LogInfo($"[{info.Name}] loaded");

                    FontBundle tmp = new()
                    {
                        Normal = bundle.LoadAsset<TMP_FontAsset>(ResourcePath.NormalFont),
                        Transmit = bundle.LoadAsset<TMP_FontAsset>(ResourcePath.TransmitFont)
                    };

                    if (tmp.Normal)
                    {
                        tmp.BundleName = info.Name;
                        tmp.Normal.name = $"{info.Name}(Normal)";
                        Plugin.LogInfo($"[{info.Name}] Normal font found ({tmp.Normal.name})");
                    }
                    if (tmp.Transmit)
                    {
                        tmp.BundleName = info.Name;
                        tmp.Transmit.name = $"{info.Name}(Transmit)";
                        Plugin.LogInfo($"[{info.Name}] Transmit font found ({tmp.Transmit.name})");
                    }

                    if (tmp.BundleName == null)
                    {
                        throw new Exception($"Not included recognizable font");
                    }

                    fontBundles.Add(tmp);
                    sucessCount += 1;
                }
                catch (Exception e)
                {
                    Plugin.LogError($"[{info.Name}] load failed: {e.Message}");
                    failCount += 1;
                }
            }

            normalRegex = new Regex(Plugin.configNormalRegexPattern.Value);
            transmitRegex = new Regex(Plugin.configTransmitRegexPattern.Value);

            StringBuilder stringBuilder = new();
            stringBuilder.Append($"{sucessCount} fonts loaded");
            if (failCount > 0) stringBuilder.Append($", {failCount} fonts load failed");
            Plugin.LogInfo(stringBuilder.ToString());
        }
        catch (Exception e)
        {
            Plugin.LogError(e.ToString());
        }
    }

    [HarmonyPrefix, HarmonyPatch(typeof(TMP_FontAsset), "Awake")]
    static void PatchFontAwake(TMP_FontAsset __instance)
    {
        __instance.material.SetFloat("_UnderlayDilate", 1f);
        __instance.material.SetFloat("_UnderlayOffsetX", 0.1f);
        string fontName = __instance.name;

        if (normalRegex.IsMatch(fontName))
        {
            if (!Plugin.configNormalIngameFont.Value)
            {
                DisableFont(__instance);
            }

            int patchCount = 0;
            foreach (FontBundle bundle in fontBundles)
            {
                if (!bundle.Normal) continue;
                if (__instance.fallbackFontAssetTable.Contains(bundle.Normal)) continue;

                __instance.fallbackFontAssetTable.Add(bundle.Normal);
                patchCount += 1;
            }

            if (patchCount > 0)
            {
                Plugin.LogInfo($"[{fontName}] font patched (Normal)");
            }
            return;
        }

        if (transmitRegex.IsMatch(fontName))
        {
            if (!Plugin.configTransmitIngameFont.Value)
            {
                DisableFont(__instance);
            }

            int patchCount = 0;
            foreach (FontBundle bundle in fontBundles)
            {
                if (!bundle.Transmit) continue;
                if (__instance.fallbackFontAssetTable.Contains(bundle.Transmit)) continue;

                __instance.fallbackFontAssetTable.Add(bundle.Transmit);
                patchCount += 1;
            }

            if (patchCount > 0)
            {
                Plugin.LogInfo($"[{fontName}] font patched (Transmit)");
            }
            return;
        }

        Plugin.LogWarning($"[{fontName}] not patched");
    }

    [HarmonyPostfix, HarmonyPatch(typeof(TMP_Text), "font", MethodType.Setter)]
    static void PatchTextFontSetter(TMP_FontAsset value)
    {
        PatchFontAwake(value);
    }

    [HarmonyPrefix, HarmonyPatch(typeof(TextMeshProUGUI), "Awake")]
    static void PatchTextAwake(TextMeshProUGUI __instance)
    {
        if (__instance.font == null) return;

        PatchFontAwake(__instance.font);
    }

    static void DisableFont(TMP_FontAsset font)
    {
        font.characterLookupTable.Clear();
        font.atlasPopulationMode = AtlasPopulationMode.Static;
    }
}