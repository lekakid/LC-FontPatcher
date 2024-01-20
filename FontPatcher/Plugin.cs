using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace FontPatcher;

class PluginInfo
{
    public const string GUID = "lekakid.lcfontpatcher";
    public const string Name = "FontPatcher";
    public const string Version = "1.2.0";
}

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
class Plugin : BaseUnityPlugin
{
    public static ConfigEntry<bool> configNormalIngameFont;
    public static ConfigEntry<bool> configTransmitIngameFont;
    public static ConfigEntry<string> configNormalRegexPattern;
    public static ConfigEntry<string> configTransmitRegexPattern;
    public static ConfigEntry<string> configFontAssetPath;
    public static ConfigEntry<bool> configDebugLog;

    public static Plugin Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        configNormalIngameFont = Config.Bind(
            "General",
            "UsingNormalIngameFont",
            true,
            "Using in-game default normal font"
        );

        configTransmitIngameFont = Config.Bind(
            "General",
            "UsingTransmitIngameFont",
            true,
            "Using in-game default normal font"
        );

        configNormalRegexPattern = Config.Bind(
            "Regex",
            "NormalFontNameRegex",
            @"^(b|DialogueText|3270.*)$",
            "Normally, you don't neet to change it"
        );

        configTransmitRegexPattern = Config.Bind(
            "Regex",
            "TransmitFontNameRegex",
            @"^edunline.*$",
            "Normally, you don't neet to change it"
        );

        configFontAssetPath = Config.Bind(
            "Path",
            "FontAssetsPath",
            @"FontPatcher\default"
        );

        configDebugLog = Config.Bind(
            "Debug",
            "Log",
            false
        );

        FontLoader.Load();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    public static void LogInfo(string msg)
    {
        if (!configDebugLog.Value) return;

        Instance.Logger.LogInfo(msg);
    }

    public static void LogWarning(string msg)
    {
        if (!configDebugLog.Value) return;

        Instance.Logger.LogWarning(msg);
    }

    public static void LogError(string msg)
    {
        Instance.Logger.LogError(msg);
    }
}
