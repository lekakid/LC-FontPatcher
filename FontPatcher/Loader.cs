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
        Plugin.Instance.LogInfo(__instance.font.name);
        string prevFontName = __instance.font.name;

        switch (prevFontName)
        {
            case "3270-Regular SDF":
                __instance.font = NormalFont;
                break;
            default:
                break;
        }
    }
}