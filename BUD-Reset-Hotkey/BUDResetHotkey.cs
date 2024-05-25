using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using CrusadersGame.GameScreen;

namespace BUDResetHotkey
{
    [BepInPlugin("rathkey.ic.budresethotkey", "BUD Reset Hotkey", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class BUDResetHotkey : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.budresethotkey");

        void Awake()
        {
            harmony.PatchAll(typeof(BUDResetHotkeyPatch));
            Debug.Log("BUDResetHotkey mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(HotKeyManager), "CheckForDialog")]
    public static class BUDResetHotkeyPatch
    {
        static bool Prefix(HotKeyManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                var controller = AccessTools.Field(typeof(HotKeyManager), "controller").GetValue(__instance) as CrusadersGameController;
                if (controller != null)
                {
                    controller.ActiveCampaignData.InitActive();
                    return false;
                }
            }
            return true;
        }
    }
}