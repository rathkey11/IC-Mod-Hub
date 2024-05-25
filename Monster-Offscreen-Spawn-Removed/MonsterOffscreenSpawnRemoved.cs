using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen;

namespace MonsterOffscreenSpawnRemoved
{
    [BepInPlugin("rathkey.ic.monsteroffscreenspawnremoved", "Monster Offscreen Spawn Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class MonsterOffscreenSpawnRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.monsteroffscreenspawnremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(MonsterOffscreenSpawnRemovedPatch));
            Debug.Log("MonsterOffscreenSpawnRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(GamePlayAreaRect))]
    public static class MonsterOffscreenSpawnRemovedPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("SpawnX", MethodType.Getter)]
        public static bool PrefixSpawnX(ref int __result, GamePlayAreaRect __instance)
        {
            var playArea = (Rect)AccessTools.Field(typeof(GamePlayAreaRect), "playArea").GetValue(__instance);
            __result = (int)((double)playArea.xMax + (double)__instance.PlayAreaScaleFactor);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch("TargetX", MethodType.Getter)]
        public static bool PrefixTargetX(ref int __result, GamePlayAreaRect __instance)
        {
            var playArea = (Rect)AccessTools.Field(typeof(GamePlayAreaRect), "playArea").GetValue(__instance);
            __result = (int)((double)playArea.x - (double)__instance.PlayAreaScaleFactor);
            return false;
        }
    }
}
