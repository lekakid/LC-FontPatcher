using BepInEx;

namespace FontPatcher;

class PluginInfo
{
    public const string GUID = "lekakid.lcfontpatcher";
    public const string Name = "FontPatcher";
    public const string Version = "1.0.0";
}

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {PluginInfo.GUID} is loaded!");
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
