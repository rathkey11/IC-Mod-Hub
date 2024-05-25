using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using CrusadersGame.GameScreen;

namespace DPSBreakdownHotkey
{
    [BepInPlugin("rathkey.ic.dpsbreakdownhotkey", "DPS Breakdown Hotkey", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class DPSBreakdownHotkey : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.dpsbreakdownhotkey");

        void Awake()
        {
            harmony.PatchAll(typeof(DPSBreakdownHotkeyPatch));
            Debug.Log("DPSBreakdownHotkey mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(HotKeyManager), "CheckForDialog")]
    public static class DPSBreakdownHotkeyPatch
    {
        static bool Prefix(HotKeyManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                var controller = AccessTools.Field(typeof(HotKeyManager), "controller").GetValue(__instance) as CrusadersGameController;
                if (controller != null)
                {
                    {
                        new EffectBreakdown(controller).ProduceBreakdown();
                    }
                    return false;
                }
            }
            return true;
        }
    }
}