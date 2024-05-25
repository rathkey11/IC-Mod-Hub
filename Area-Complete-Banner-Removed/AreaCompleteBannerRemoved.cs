using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen.UIComponents.TopBar;

namespace AreaCompleteBannerRemoved
{
    [BepInPlugin("rathkey.ic.areacompletebannerremoved", "Area Complete Banner Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class AreaCompleteBannerRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.areacompletebannerremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(AreaCompleteBannerRemovedPatch));
            Debug.Log("AreaCompleteBannerRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(TopBar), "ShowAreaCompleteText")]
    public static class AreaCompleteBannerRemovedPatch
    {
        static bool Prefix()
        {
            return false;
        }
    }
}