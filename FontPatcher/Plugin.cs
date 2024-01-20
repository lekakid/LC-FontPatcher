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
    public ConfigEntry<bool> configNormalIngameFont;
    public ConfigEntry<bool> configTransmitIngameFont;

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

        FontLoader.Load(Info.Location);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    public void LogInfo(string msg)
    {
        Logger.LogInfo(msg);
    }

    public void LogWarning(string msg)
    {
        Logger.LogWarning(msg);
    }

    public void LogError(string msg)
    {
        Logger.LogError(msg);
    }
}
