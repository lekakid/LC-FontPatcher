using System;
using System.IO;
using System.Collections.Generic;
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

    public static void Load(string location)
    {
        try
        {
            string dirName = Path.GetDirectoryName(location);
            string fontsPath = Path.Combine(dirName, ResourcePath.FontPath);
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
        string prevFontName = __instance.name.Split(" ")[0];

        switch (prevFontName)
        {
            case "3270-Regular":
            case "3270-HUDIngame":
            case "3270-HUDIngameB":
            case "DialogueText":
            case "b":
                if (!Plugin.configNormalIngameFont.Value)
                {
                    DisableFont(__instance);
                }
                foreach (FontBundle bundle in fontBundles)
                {
                    if (!bundle.Normal) continue;
                    __instance.fallbackFontAssetTable.Add(bundle.Normal);
                }
                break;
            case "edunline":
                if (!Plugin.configTransmitIngameFont.Value)
                {
                    DisableFont(__instance);
                }
                foreach (FontBundle bundle in fontBundles)
                {
                    if (!bundle.Transmit) continue;
                    __instance.fallbackFontAssetTable.Add(bundle.Transmit);
                }
                break;
            default:
                break;
        }
    }

    static void DisableFont(TMP_FontAsset font)
    {
        font.characterLookupTable.Clear();
        font.atlasPopulationMode = AtlasPopulationMode.Static;
    }
}