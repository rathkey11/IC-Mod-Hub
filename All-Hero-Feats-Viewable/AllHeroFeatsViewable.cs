using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Defs;

namespace AllHeroFeatsViewable
{
    [BepInPlugin("rathkey.ic.allherofeatsviewable", "All Hero Feats Viewable", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class AllHeroFeatsViewable : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.allherofeatsviewable");

        void Awake()
        {
            harmony.PatchAll(typeof(AllHeroFeatsViewablePatch));
            harmony.PatchAll(typeof(AllHeroFeatsViewablePatch2));
            Debug.Log("AllHeroFeatsViewable mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(HeroFeatDef), "ShowUnlocked", MethodType.Getter)]
    public static class AllHeroFeatsViewablePatch
    {
        static bool Prefix(HeroFeatDef __instance, ref bool __result)
        {
            __result = true;
            return false;
        }
    }
    [HarmonyPatch(typeof(HeroFeatDef), "ShowLocked", MethodType.Getter)]
    public static class AllHeroFeatsViewablePatch2
    {
        static bool Prefix(HeroFeatDef __instance, ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
