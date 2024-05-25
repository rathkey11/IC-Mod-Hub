using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using CrusadersGame.GameScreen;

namespace PauseHotkey
{
    [BepInPlugin("rathkey.ic.pausehotkey", "Pause Hotkey", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class PauseHotkey : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.pausehotkey");

        void Awake()
        {
            harmony.PatchAll(typeof(PauseHotkeyPatch));
            Debug.Log("PauseHotkey mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(HotKeyManager), "CheckForDialog")]
    public static class PauseHotkeyPatch
    {
        static bool Prefix(HotKeyManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                var controller = AccessTools.Field(typeof(HotKeyManager), "controller").GetValue(__instance) as CrusadersGameController;
                if (controller != null)
                {
                    if (!controller.Paused)
                    {
                        controller.Pause(false);
                    }
                    else
                    {
                        controller.Resume(false);
                    }
                    return false;
                }
            }
            return true;
        }
    }
}