using System;
using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using LethalTerminalExtender.Patches;
using UnityEngine;

namespace GangeroniMods
{
    [BepInPlugin("com.BigSaltyBeans.blahaj", "Blahaj The Shark", "1.1.3")]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class Blahaj : BaseUnityPlugin
    {
        public static AssetBundle BlahajTheShark;

        private void Awake()
        {
            var harmony = new Harmony("com.BigSaltyBeans.blahaj");
            harmony.PatchAll();

            string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string bundlePath = Path.Combine(assemblyLocation, "blahajtheshark");

            Logger.LogInfo("[Blahaj] Attempting to load asset bundle at: " + bundlePath);

            if (!File.Exists(bundlePath))
            {
                Logger.LogError("[Blahaj] Asset bundle not found at: " + bundlePath);
                return;
            }

            try
            {
                Debug.Log("Loading Blahaj Item...");

                BlahajTheShark = AssetBundle.LoadFromFile(bundlePath);
                Logger.LogInfo("Loading from path: " + bundlePath);

                if (BlahajTheShark == null)
                {
                    Logger.LogError("Failed to load custom assets.");
                    return;
                }

                int blahajRarity = 100;
                Item blahajItem = BlahajTheShark.LoadAsset<Item>("assets/blahaj/blahaj.asset");
                if (blahajItem == null)
                {
                    Logger.LogError("Failed to load Blahaj Item.");
                    return;
                }

                LethalLib.Modules.Utilities.FixMixerGroups(blahajItem.spawnPrefab);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(blahajItem.spawnPrefab);
                LethalLib.Modules.Items.RegisterScrap(blahajItem, 999, LethalLib.Modules.Levels.LevelTypes.All);
                
                
                TerminalExtenderUtils.addQuickCommand("blahajtest", "Spawn Blahaj", true, (Terminal term, TerminalNode node) =>
                {
                    TerminalNode responseNode = ScriptableObject.CreateInstance<TerminalNode>();
                    responseNode.displayText = "Attempting to spawn Blåhaj!\n";
                    responseNode.clearPreviousText = true;
                    responseNode.maxCharactersToType = 10;

                    if (BlahajTheShark == null)
                    {
                        responseNode.displayText += "\nAssetBundle is null.";
                        term.LoadNewNode(responseNode);
                        return;
                    }

                    var blahajItem = BlahajTheShark.LoadAsset<Item>("assets/blahaj/blahaj.asset");
                    if (blahajItem == null)
                    {
                        responseNode.displayText += "\nItem asset not found!";
                        term.LoadNewNode(responseNode);
                        return;
                    }

                    if (blahajItem.spawnPrefab == null)
                    {
                        responseNode.displayText += "\nspawnPrefab is null!";
                        term.LoadNewNode(responseNode);
                        return;
                    }

                    // Find player position and spawn the prefab
                    var player = GameNetworkManager.Instance.localPlayerController;
                    var spawnPos = player.transform.position + player.transform.forward * -2f;

                    var spawned = Instantiate(blahajItem.spawnPrefab, spawnPos, Quaternion.identity);
                    responseNode.displayText += "\nBlåhaj spawned!";
                    term.LoadNewNode(responseNode);
                });

                
                
                Logger.LogInfo("Blahaj loaded.");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception in Blahaj plugin: " + ex.ToString());
            }
        }
    }
}