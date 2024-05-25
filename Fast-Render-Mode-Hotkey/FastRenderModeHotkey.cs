using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using CrusadersGame.GameScreen;

namespace FastRenderModeHotkey
{
    [BepInPlugin("rathkey.ic.fastrendermodehotkey", "Fast Render Mode Hotkey", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class FastRenderModeHotkey : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.fastrendermodehotkey");

        void Awake()
        {
            harmony.PatchAll(typeof(FastRenderModeHotkeyPatch));
            Debug.Log("FastRenderModeHotkey mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(HotKeyManager), "CheckForDialog")]
    public static class FastRenderModeHotkeyPatch
    {
        static bool Prefix(HotKeyManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                var controller = AccessTools.Field(typeof(HotKeyManager), "controller").GetValue(__instance) as CrusadersGameController;

                if (controller != null)
                {
                    controller.GameScreen.UIController.TopBar.Minimized = !controller.GameScreen.UIController.TopBar.Minimized;
                    Debug.Log("TopBar Minimized: " + controller.GameScreen.UIController.TopBar.Minimized);

                    controller.GameScreen.UIController.BottomBar.Minimized = !controller.GameScreen.UIController.BottomBar.Minimized;
                    Debug.Log("BottomBar Minimized: " + controller.GameScreen.UIController.BottomBar.Minimized);

                    controller.GameScreen.AreaBackgroundLayer.Visible = !controller.GameScreen.AreaBackgroundLayer.Visible;
                    Debug.Log("AreaBackgroundLayer Visible: " + controller.GameScreen.AreaBackgroundLayer.Visible);

                    controller.GameScreen.GameInstance.Screen.Settings.FastRenderMode = !controller.GameScreen.GameInstance.Screen.Settings.FastRenderMode;
                    Debug.Log("FastRenderMode: " + controller.GameScreen.GameInstance.Screen.Settings.FastRenderMode);

                    return false;
                }
            }
            return true;
        }
    }
}