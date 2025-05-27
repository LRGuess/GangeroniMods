using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using LethalTerminalExtender.Patches;

namespace GangeroniMods
{
    [BepInPlugin("com.BigSaltyBeans.togglelights", "Toggle Ship Lights", "1.0.2")]
    public class ToggleShipLights : BaseUnityPlugin
    {
        public static ShipLights shipInteriorLights;
        public static bool lightsOn = true;

        private void Awake()
        {
            Logger.LogInfo("ToggleShipLights loaded.");
            Harmony harmony = new Harmony("com.BigSaltyBeans.togglelights");
            harmony.PatchAll();
            
            TerminalExtenderUtils.addQuickCommand("lights", "Toggle Ship Lights", true, (Terminal term, TerminalNode node) =>
            {
                TerminalNode responseNode = ScriptableObject.CreateInstance<TerminalNode>();
                responseNode.displayText = "Lights Toggled!\n";
                responseNode.clearPreviousText = true;
                responseNode.maxCharactersToType = 15;
                ToggleLights();
                term.LoadNewNode(responseNode);
            });
        }

        private void Start()
        {
            FindShipLights();
        }

        private void FindShipLights()
        {
            var obj = GameObject.Find("ShipElectricLights");
            if (obj != null)
            {
                shipInteriorLights = obj.GetComponent<ShipLights>();
                Logger.LogInfo("ShipElectricLights found.");
            }
        }

        public static void ToggleLights()
        {
            if (shipInteriorLights == null)
            {
                shipInteriorLights = GameObject.Find("ShipElectricLights")?.GetComponent<ShipLights>();
                if (shipInteriorLights == null)
                {
                    Debug.LogWarning("Ship lights not found.");
                    return;
                }
            }

            shipInteriorLights.ToggleShipLights();
            lightsOn = !lightsOn;

            Debug.Log($"[ToggleShipLights] Lights toggled {(lightsOn ? "ON" : "OFF")}");
        }
    }
}
