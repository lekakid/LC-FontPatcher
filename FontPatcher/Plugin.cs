using System.Reflection;
using BepInEx;
using HarmonyLib;

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
    public static Plugin Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

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
