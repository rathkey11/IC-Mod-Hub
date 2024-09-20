using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Dialogs.Inventory;
using System.Reflection;
using UnityGameEngine.Utilities;

namespace FastPotionUse
{
    [BepInPlugin("rathkey.ic.fastpotionuse", "Fast Potion Use", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class FastPotionUse : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.fastpotionuse");

        void Awake()
        {
            harmony.PatchAll(typeof(FastPotionUsePatch));
            Debug.Log("FastPotionUse mod loaded");
        }
    }

    [HarmonyPatch]
    public static class FastPotionUsePatch
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(InventoryItem), "OnClick", new[] { typeof(int), typeof(int) });
        }

        static void Prefix(InventoryItem __instance, ref bool __runOriginal)
        {
            var currDataField = AccessTools.Field(typeof(InventoryItem), "currData");
            var currData = (InventoryItemData)currDataField.GetValue(__instance);

            if (currData.Type == InventoryItemData.ItemType.Buff && currData.BuffDef != null && (!currData.BuffDef.HasUseCounts || Utils.ShiftKeyPressed()))
            {
                var itemSelectConfirmedMethod = AccessTools.Method(typeof(InventoryItem), "ItemSelectConfirmed");
                itemSelectConfirmedMethod.Invoke(__instance, new object[] { 1 });

                __runOriginal = false;
            }
        }
    }
}