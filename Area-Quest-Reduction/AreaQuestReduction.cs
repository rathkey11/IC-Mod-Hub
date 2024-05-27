using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen;

namespace AreaQuestReduction
{
    [BepInPlugin("rathkey.ic.areaquestreduction", "Area Quest Reduction", "0.2.0")]
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

    [HarmonyPatch(typeof(Area), "CalculateQuestRewardMultFromEffectKey")]
    public static class AreaQuestReductionPatch
    {
        static bool Prefix(ref double inputQuestRewardMult)
        {
            inputQuestRewardMult = 25.0;
            return true;
        }
    }
}