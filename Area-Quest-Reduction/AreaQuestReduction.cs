using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen;

namespace AreaQuestReduction
{
    [BepInPlugin("rathkey.ic.areaquestreduction", "Area Quest Reduction", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class AreaQuestReduction : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.areaquestreduction");

        void Awake()
        {
            harmony.PatchAll(typeof(AreaQuestReductionPatch));
            Debug.Log("AreaQuestReduction mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(AreaLevel), "AddQuestProgress")]
    public static class AreaQuestReductionPatch
    {
        static bool Prefix(CrusadersGame.GameScreen.AreaLevel __instance)
        {
            Debug.Log("Before patch: QuestRemaining = " + __instance.QuestRemaining);
            if (__instance.QuestRemaining > 0)
            {
                __instance.QuestRemaining = 1;
                Debug.Log("After patch: QuestRemaining = " + __instance.QuestRemaining);
            }
            return true;
        }
    }
}
