using HarmonyLib;
using UnityEngine;
using UnityGameEngine.Utilities;
using BepInEx;
using CrusadersGame.GameScreen.UIComponents.TopBar.ObjectiveProgress;
using CrusadersGame.GameScreen;
using System;

namespace AltClick50Areas
{
    [BepInPlugin("rathkey.ic.altclick50areas", "Alt Click 50 Areas", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class AltClick50Areas : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.altclick50areas");

        void Awake()
        {
            harmony.PatchAll(typeof(AltClick50AreasRight));
            harmony.PatchAll(typeof(AltClick50AreasLeft));
            Debug.Log("AltClick50Areas mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(AreaLevelBar), "RightClicked")]
    public static class AltClick50AreasRight
    {
        static bool Prefix(AreaLevelBar __instance)
        {
            if (Utils.AltKeyPressed())
            {
                var firstMapNodeLevel = (int)AccessTools.Field(typeof(AreaLevelBar), "firstMapNodeLevel").GetValue(__instance);
                var highestAvailableLevel = (int)AccessTools.Field(typeof(AreaLevelBar), "highestAvailableLevel").GetValue(__instance);
                var currentArea = (AreaLevel)AccessTools.Field(typeof(AreaLevelBar), "currentArea").GetValue(__instance);
                int num = firstMapNodeLevel + 45;
                if (num <= highestAvailableLevel)
                {
                    __instance.Init(currentArea, highestAvailableLevel, num);
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(AreaLevelBar), "LeftClicked")]
    public static class AltClick50AreasLeft
    {
        static bool Prefix(AreaLevelBar __instance)
        {
            if (Utils.AltKeyPressed())
            {
                var firstMapNodeLevel = (int)AccessTools.Field(typeof(AreaLevelBar), "firstMapNodeLevel").GetValue(__instance);
                var highestAvailableLevel = (int)AccessTools.Field(typeof(AreaLevelBar), "highestAvailableLevel").GetValue(__instance);
                var currentArea = (AreaLevel)AccessTools.Field(typeof(AreaLevelBar), "currentArea").GetValue(__instance);
                if (firstMapNodeLevel > 1)
                {
                    __instance.Init(currentArea, highestAvailableLevel, Math.Max(1, firstMapNodeLevel - 45));
                }
            }
            return true;
        }
    }
}
