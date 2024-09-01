using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen;
using System.Reflection;

namespace MonsterOffscreenSpawnRemoved
{
    [BepInPlugin("rathkey.ic.monsteroffscreenspawnremoved", "Monster Offscreen Spawn Removed", "0.2.0")]
    [BepInProcess("IdleDragons.exe")]
    public class MonsterOffscreenSpawnRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.monsteroffscreenspawnremoved");

        void Awake()
        {
            harmony.PatchAll();
            Debug.Log("MonsterOffscreenSpawnRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(GamePlayAreaRect))]
    public static class MonsterOffscreenSpawnRemovedPatch1
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

    [HarmonyPatch(typeof(Monster), "UpdateMovement")]
    public static class MonsterOffscreenSpawnRemovedPatch2
    {
        private static readonly FieldInfo controllerField = typeof(Monster).GetField("controller", BindingFlags.NonPublic | BindingFlags.Instance);

        [HarmonyPostfix]
        public static void Postfix(Monster __instance)
        {
            var controller = (CrusadersGameController)controllerField.GetValue(__instance);
            if (__instance.X <= controller.PlayAreaRect.PlayAreaRect.xMax)
            {
                __instance.DoneTransitioning();
            }
        }
    }
}
