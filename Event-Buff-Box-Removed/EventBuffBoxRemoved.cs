using HarmonyLib;
using UnityEngine;
using BepInEx;
using System.Reflection;
using CrusadersGame.User;

namespace EventBuffBoxRemoved
{
    [BepInPlugin("rathkey.ic.eventbuffboxremoved", "Event Buff Box Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class EventBuffBoxRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.eventbuffboxremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(EventBuffBoxRemovedPatch));
            Debug.Log("EventBuffBoxRemoved mod loaded");
        }
    }

    [HarmonyPatch]
    public static class EventBuffBoxRemovedPatch
    {
        static MethodBase TargetMethod()
        {
            return typeof(EventBuffsHandler).GetMethod("AddBuffBoxes", BindingFlags.NonPublic | BindingFlags.Instance);
        }
        static bool Prefix()
        {
            return false;
        }
    }
}