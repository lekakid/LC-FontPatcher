using System;
using System.IO;
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
            DirectoryInfo di = new DirectoryInfo(fontsPath);
            FileInfo[] fileInfos = di.GetFiles("*");

            foreach (FileInfo info in fileInfos)
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(info.FullName);

                FontBundle tmp = new()
                {
                    Normal = bundle.LoadAsset<TMP_FontAsset>(ResourcePath.NormalFont),
                    Transmit = bundle.LoadAsset<TMP_FontAsset>(ResourcePath.TransmitFont)
                };

                if (tmp.Normal) tmp.Normal.name = $"{info.Name}(Normal)";
                if (tmp.Transmit) tmp.Transmit.name = $"{info.Name}(Transmit)";

                fontBundles.Add(tmp);
            }

            normalRegex = new Regex(Plugin.configNormalRegexPattern.Value);
            transmitRegex = new Regex(Plugin.configTransmitRegexPattern.Value);

            Plugin.LogInfo($"Font loaded!");
        }
        catch (Exception e)
        {
            Plugin.LogError(e.Message);
        }
    }

    [HarmonyPrefix, HarmonyPatch(typeof(TMP_FontAsset), "Awake")]
    static void PrefixAwake(TMP_FontAsset __instance)
    {
        string fontName = __instance.name;

        if (normalRegex.IsMatch(fontName))
        {
            if (!Plugin.configNormalIngameFont.Value)
            {
                DisableFont(__instance);
            }
            foreach (FontBundle bundle in fontBundles)
            {
                if (!bundle.Normal) continue;
                __instance.fallbackFontAssetTable.Add(bundle.Normal);
            }

            Plugin.LogInfo($"[{fontName}] font patched (Normal)");
            return;
        }

        if (transmitRegex.IsMatch(fontName))
        {
            if (!Plugin.configTransmitIngameFont.Value)
            {
                DisableFont(__instance);
            }
            foreach (FontBundle bundle in fontBundles)
            {
                if (!bundle.Transmit) continue;
                __instance.fallbackFontAssetTable.Add(bundle.Transmit);
            }

            Plugin.LogInfo($"[{fontName}] font patched (Transmit)");
            return;
        }

        Plugin.LogWarning($"[{fontName}] not patched");
    }

    static void DisableFont(TMP_FontAsset font)
    {
        font.characterLookupTable.Clear();
        font.atlasPopulationMode = AtlasPopulationMode.Static;
    }
}