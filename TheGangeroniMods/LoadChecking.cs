using BepInEx;
using BepInEx.Logging;

namespace GangeroniMods
{
    [BepInPlugin("com.BigSaltyBeans.modloadingchecker", "Mod Loading Checker", "1.0.0")]
    public class LoadChecking : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("Mod Checker Passed All Checks");
        }
    }
}