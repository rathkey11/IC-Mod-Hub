using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen.Goobers;
using System.Reflection;

namespace GoldAnimationsRemoved
{
    [BepInPlugin("rathkey.ic.goldanimationsremoved", "Gold Animations Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class GoldAnimationsRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.goldanimationsremoved");

        void Awake()
        {
            harmony.PatchAll();
            Debug.Log("GoldAnimationsRemoved mod loaded");
        }
    }

    [HarmonyPatch(typeof(Goober), "PopOut")]
    public static class PopOutRemoved
    {
        static bool Prefix(Goober __instance)
        {
            AccessTools.Method(typeof(Goober), "AnimationComplete").Invoke(__instance, null);
            return false;
        }
    }

    [HarmonyPatch(typeof(Goober), "Drop")]
    public static class DropRemoved
    {
        static bool Prefix(Goober __instance)
        {
            AccessTools.Method(typeof(Goober), "AnimationComplete").Invoke(__instance, null);
            return false;
        }
    }

    [HarmonyPatch]
    public static class HopRemoved
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Goober), "Hop");
        }

        static bool Prefix(Goober __instance)
        {
            AccessTools.Method(typeof(Goober), "AnimationComplete").Invoke(__instance, null);
            return false;
        }
    }

    [HarmonyPatch(typeof(Goober), "Fly")]
    public static class FlyTimeRemoved
    {
        static void Prefix(object __instance)
        {
            var gooberDataField = AccessTools.Field(typeof(Goober), "gooberData");
            var gooberData = (Goober.GooberData)gooberDataField.GetValue(__instance);

            gooberData.FlyTime = 0f;
        }
    }
}