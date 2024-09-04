using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Dialogs;
using System.Collections.Generic;

namespace SpeedUpChestOpening
{
    [BepInPlugin("rathkey.ic.speedupchestopening", "Speed Up Chest Opening", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class SpeedUpChestOpening : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.speedupchestopening");

        void Awake()
        {
            harmony.PatchAll(typeof(SpeedUpChestOpeningPatch));
            Debug.Log("SpeedUpChestOpening mod loaded");
        }
    }

    [HarmonyPatch(typeof(OpenChestDialog), "ShowCards")]
    public static class SpeedUpChestOpeningPatch
    {
        static bool Prefix(OpenChestDialog __instance)
        {
            var categorizedLootField = AccessTools.Field(typeof(OpenChestDialog), "categorizedLoot");
            var categorizedLoot = (Dictionary<AllChestResultsDialog.Category, ChestCategoryPanel.StackedLoot>)categorizedLootField.GetValue(__instance);

            if (categorizedLoot != null)
            {
                __instance.state = OpenChestDialog.State.CARDS_REVEALED;
                __instance.DoneButtonClicked();
                return false;
            }
            return true;
        }
    }
}