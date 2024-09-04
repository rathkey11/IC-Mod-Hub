using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen.Familiars;
using UnityGameEngine.Display;
using System;

namespace FamiliarAnimationRemoved
{
    [BepInPlugin("rathkey.ic.familiaranimationremoved", "Familiar Animation Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class FamiliarAnimationRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.familiaranimationremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(FamiliarAnimationRemovedPatch));
            Debug.Log("FamiliarAnimationRemoved mod loaded");
        }
    }

    [HarmonyPatch(typeof(Familiar))]
    public static class FamiliarAnimationRemovedPatch
    {
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPostfix]
        static void ConstructorPostfix(Familiar __instance)
        {
            var familiarGraphicField = AccessTools.Field(typeof(Familiar), "familiarGraphic");
            var familiarGraphic = (Drawable)familiarGraphicField.GetValue(__instance);
            familiarGraphic.StopAtEnd = true;
        }

        [HarmonyPatch("Idle", new Type[] { })]
        [HarmonyPostfix]
        static void IdlePostfix(Familiar __instance)
        {
            var familiarGraphicField = AccessTools.Field(typeof(Familiar), "familiarGraphic");
            var familiarGraphic = (Drawable)familiarGraphicField.GetValue(__instance);
            familiarGraphic.StopAtEnd = true;
        }

        [HarmonyPatch("Idle", new Type[] { typeof(Drawable) })]
        [HarmonyPostfix]
        static void IdleWithDrawablePostfix(Familiar __instance)
        {
            var familiarGraphicField = AccessTools.Field(typeof(Familiar), "familiarGraphic");
            var familiarGraphic = (Drawable)familiarGraphicField.GetValue(__instance);
            familiarGraphic.StopAtEnd = true;
        }
    }
}