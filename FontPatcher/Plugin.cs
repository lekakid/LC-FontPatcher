using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace FontPatcher;

class PluginInfo
{
    public const string GUID = "lekakid.lcfontpatcher";
    public const string Name = "FontPatcher";
    public const string Version = "1.1.5";
}

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
class Plugin : BaseUnityPlugin
{
    public static ConfigEntry<bool> configNormalIngameFont;
    public static ConfigEntry<bool> configTransmitIngameFont;
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

        configDebugLog = Config.Bind(
            "Debug",
            "Log",
            false
        );

        FontLoader.Load(Info.Location);
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
        if (!configDebugLog.Value) return;

        Instance.Logger.LogError(msg);
    }
}
