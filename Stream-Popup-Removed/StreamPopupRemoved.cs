using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen;

namespace StreamPopupRemoved
{
    [BepInPlugin("rathkey.ic.streampopupremoved", "Stream Popup Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class StreamPopupRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.streampopupremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(StreamPopupRemovedPatch));
            Debug.Log("StreamPopupRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(UIController), "ShowExternalLinkNotification")]
    public static class StreamPopupRemovedPatch
    {
        static bool Prefix()
        {
            return false;
        }
    }
}