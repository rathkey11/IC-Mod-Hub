using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Effects;

namespace SpecialGuestSpurt
{
    [BepInPlugin("rathkey.ic.specialguestspurt", "Special Guest Spurt", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class SpecialGuestSpurt : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.specialguestspurt");

        void Awake()
        {
            harmony.PatchAll(typeof(SpecialGuestSpurtPatch));
            Debug.Log("SpecialGuestSpurt mod loaded");
        }
    }

    [HarmonyPatch(typeof(DS1), "targetHeroId", MethodType.Getter)]
    public static class SpecialGuestSpurtPatch
    {
        static bool Prefix(ref int __result)
        {
            __result = 43;
            return false;
        }
    }
}