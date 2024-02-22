using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Effects;
using System.Reflection;

namespace Briv9j
{
    [BepInPlugin("rathkey.ic.briv9j", "Briv 9j", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class Briv9j : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.briv9j");

        void Awake()
        {
            harmony.PatchAll(typeof(Briv9jPatch));
            Debug.Log("Briv9j mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch]
    public static class Briv9jPatch
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method("CrusadersGame.Effects.BrivUnnaturalHasteHandler:CalculateAreaSkipValues");
        }

        static void Postfix(object __instance)
        {
            AccessTools.Field(__instance.GetType(), "areaSkipChance").SetValue(__instance, 1f);
            AccessTools.Field(__instance.GetType(), "areaSkipAmount").SetValue(__instance, 9);
        }
    }
}
