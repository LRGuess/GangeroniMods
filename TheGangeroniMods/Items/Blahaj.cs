using System;
using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using LethalLib.Modules;
using LethalTerminalExtender.Patches;
using Unity.Netcode;
using UnityEngine;
using NetworkPrefabs = LethalLib.Modules.NetworkPrefabs;

namespace GangeroniMods
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Blahaj : BaseUnityPlugin
    {
        const string GUID = "com.BigSaltyBeans.Blahaj";
        const string NAME = "Blahaj The Shark";
        const string VERSION = "1.1.3";

        public static Blahaj instance;

        void Awake()
        {
            instance = this;

            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "blahajassetbundle");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetDir);

            Item blahaj = bundle.LoadAsset<Item>("Assets/Blahaj/BlahajItem.asset");
            NetworkPrefabs.RegisterNetworkPrefab(blahaj.spawnPrefab);
            Utilities.FixMixerGroups(blahaj.spawnPrefab);
            Items.RegisterScrap(blahaj, 1, Levels.LevelTypes.All);
            
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
            Logger.LogInfo("Loaded Blahaj");
        }
    }
}