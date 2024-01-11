using System;
using TMPro;
using UnityEngine;
using HarmonyLib;
using System.IO;

namespace FontPatcher;

[HarmonyPatch]
class FontLoader
{
    static TMP_FontAsset NormalFont;

    public static void Load(string location)
    {
        try
        {
            string dirName = Path.GetDirectoryName(location);
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(dirName, "font"));
            NormalFont = bundle.LoadAsset<TMP_FontAsset>(ResourcePath.NormalFont);

            Plugin.Instance.LogInfo($"Font loaded!");
        }
        catch (Exception e)
        {
            Plugin.Instance.LogError(e.Message);
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(TextMeshProUGUI), "Awake")]
    static void PostFixAwake(TextMeshProUGUI __instance)
    {
        string prevFontName = __instance.font?.name;

        switch (prevFontName)
        {
            case "3270-Regular SDF":
            case "3270-HUDIngame":
            case "3270-HUDIngameB":
            case "3270-HUDIngame - Variant":
            case "b":
                SwapFont(__instance, NormalFont);
                break;
            default:
                break;
        }
    }

    static void SwapFont(TextMeshProUGUI instance, TMP_FontAsset nextFont)
    {
        string prevFontName = instance.font?.name;
        string target = $"{instance.transform.name}({instance.text.Substring(0, Math.Min(instance.text.Length, 10))})";

        TMP_FontAsset prevFont = instance.font;
        instance.font = nextFont;
        instance.fontMaterial = nextFont.material;
        instance.font.fallbackFontAssetTable.Add(prevFont);

        Plugin.Instance.LogInfo($"{target} | {prevFontName} => {nextFont.name}");
    }
}