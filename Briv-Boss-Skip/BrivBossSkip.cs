using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen;

namespace BrivBossSkip
{
    [BepInPlugin("rathkey.ic.brivbossskip", "Briv Boss Skip", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class BrivBossSkip : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.brivbossskip");

        void Awake()
        {
            harmony.PatchAll(typeof(BrivBossSkipPatch));
            Debug.Log("BrivBossSkip mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(AreaSkipHandler), "CanSkipBossLevels")]
    public static class BrivBossSkipPatch
    {
        static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
