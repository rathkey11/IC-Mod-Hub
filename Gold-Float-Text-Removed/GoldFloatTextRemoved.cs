using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen.VisualEffects;

namespace GoldFloatTextRemoved
{
    [BepInPlugin("rathkey.ic.goldfloattextremoved", "Gold Float Text Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class GoldFloatTextRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.goldfloattextremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(GoldFloatTextRemovedPatch));
            Debug.Log("GoldFloatTextRemoved mod loaded");
        }
    }

    [HarmonyPatch(typeof(FloatText))]
    [HarmonyPatch(MethodType.Constructor)]
    public static class GoldFloatTextRemovedPatch
    {
        static bool Prefix()
        {
            return false;
        }
    }
}