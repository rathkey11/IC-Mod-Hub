using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen.Familiars;
using CrusadersGame.Defs;

namespace FastFamiliars
{
    [BepInPlugin("rathkey.ic.fastfamiliars", "Fast Familiars", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class FastFamiliars : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.fastfamiliars");

        void Awake()
        {
            harmony.PatchAll(typeof(FastFamiliarsPatch));
            Debug.Log("FastFamiliars mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(FamiliarSlot), "ResetClickTimer")]
    public static class FastFamiliarsPatch
    {
        static bool Prefix(FamiliarSlot __instance)
        {
            // Access private fields
            var clickTimerField = AccessTools.Field(typeof(FamiliarSlot), "clickTimer");
            var familiarDisplayField = AccessTools.Field(typeof(FamiliarSlot), "familiarDisplay");
            var assignmentTypeField = AccessTools.Field(typeof(FamiliarSlot), "assignmentType");

            // Get field values
            var clickTimer = (float)clickTimerField.GetValue(__instance);
            var familiarDisplay = (FamiliarDisplay)familiarDisplayField.GetValue(__instance);
            var assignmentType = (FamiliarDef.AssignmentType)assignmentTypeField.GetValue(__instance);

            // Modify clickTimer value
            clickTimer = 0f;
            if (!familiarDisplay.GameController.GameInstance.IsActiveInstance)
            {
                clickTimer /= familiarDisplay.GetBGFamiliarRateMultiplier(assignmentType);
            }

            // Set modified value back to instance
            clickTimerField.SetValue(__instance, clickTimer);

            // Return false to skip original method
            return false;
        }
    }

}