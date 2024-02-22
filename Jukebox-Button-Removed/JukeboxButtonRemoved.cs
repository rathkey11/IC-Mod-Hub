using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen.UIComponents;

namespace JukeboxButtonRemoved
{
    [BepInPlugin("rathkey.ic.jukeboxbuttonremoved", "Jukebox Button Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class JukeboxButtonRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.jukeboxbuttonremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(ShowJukeboxButtonPatch));
            harmony.PatchAll(typeof(HideJukeboxButtonPatch));
            Debug.Log("JukeboxButtonRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(InventoryButton), "ShowJukeboxButton")]
    public static class ShowJukeboxButtonPatch
    {
        static bool Prefix(InventoryButton __instance)
        {
            var hideMethod = AccessTools.Method(typeof(InventoryButton), "HideJukeboxButtonInternal");
            hideMethod.Invoke(__instance, new object[] { true });
            return false; // Skip original method
        }
    }

    [HarmonyPatch(typeof(InventoryButton), "HideJukeboxButton")]
    public static class HideJukeboxButtonPatch
    {
        static bool Prefix(InventoryButton __instance)
        {
            var hideMethod = AccessTools.Method(typeof(InventoryButton), "HideJukeboxButtonInternal");
            hideMethod.Invoke(__instance, new object[] { true });
            return false; // Skip original method
        }
    }
}
