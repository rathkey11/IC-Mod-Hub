using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using CrusadersGame.GameScreen;

namespace ClearFormationHotkey
{
    [BepInPlugin("rathkey.ic.clearformationhotkey", "Clear Formation Hotkey", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class ClearFormationHotkey : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.clearformationhotkey");

        void Awake()
        {
            harmony.PatchAll(typeof(ClearFormationHotkeyPatch));
            Debug.Log("ClearFormationHotkey mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(HotKeyManager), "CheckForDialog")]
    public static class ClearFormationHotkeyPatch
    {
        static bool Prefix(HotKeyManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                var controller = AccessTools.Field(typeof(HotKeyManager), "controller").GetValue(__instance) as CrusadersGameController;
                if (controller != null)
                {
                    {
                        controller.GameInstance.FormationSaveHandler.LoadFormation(null, true);
                    }
                    return false;
                }
            }
            return true;
        }
    }
}