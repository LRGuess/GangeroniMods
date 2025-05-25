using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using LethalLib.Modules;
using LethalLib.Extras;

namespace GangeroniMods
{
    [BepInPlugin("com.BigSaltyBeans.Blahaj", "Custom Item Mod", "1.0.0")]
    [BepInDependency(LethalLib.Plugin.ModGUID)] 
    public class Blahaj : BaseUnityPlugin
    {
        public static AssetBundle BlahajTheShark;
        public ManualLogSource mls;
        private void Awake()
        {
            Debug.Log("Loading Blahaj Item...");
            string sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            BlahajTheShark = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "blahajtheshark"));
            if (BlahajTheShark == null) {
                mls.LogError("Failed to load custom assets."); // ManualLogSource for your plugin
                return;
            }

            int blahajRarity = 100;
            Item blahajItem = BlahajTheShark.LoadAsset<Item>("assets/blahaj/blahaj.asset");
            LethalLib.Modules.Utilities.FixMixerGroups(blahajItem.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(blahajItem.spawnPrefab);
            LethalLib.Modules.Items.RegisterScrap(blahajItem, blahajRarity, LethalLib.Modules.Levels.LevelTypes.All);
        }
    }
}