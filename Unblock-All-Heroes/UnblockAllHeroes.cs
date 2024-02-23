using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameChanges;

namespace UnblockAllHeroes
{
    [BepInPlugin("rathkey.ic.unblockallheroes", "Unblock All Heroes", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class UnblockAllHeroes : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.unblockallheroes");

        void Awake()
        {
            harmony.PatchAll(typeof(UnblockAllHeroesPatch));
            Debug.Log("UnblockAllHeroes mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(GameRules.AllowedHeroesInfo), "IsHeroAvailable")]
    public static class UnblockAllHeroesPatch
    {
        static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
