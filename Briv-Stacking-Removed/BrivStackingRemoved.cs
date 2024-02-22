using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Effects;
using System.Reflection;
using System;

namespace BrivStackingRemoved
{
    [BepInPlugin("rathkey.ic.brivstackingremoved", "Briv Stacking Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class BrivStackingRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.brivstackingremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(BrivStackingRemovedPatch));
            Debug.Log("BrivStackingRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch]
    public static class BrivStackingRemovedPatch
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(EffectStacks), "RemoveStacks", new Type[] { typeof(long) });
        }

        static bool Prefix()
        {
            return false;
        }
    }
}
