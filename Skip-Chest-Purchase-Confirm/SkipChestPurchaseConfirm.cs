using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Dialogs.Shop3;
using CrusadersGame.Defs;
using System.Collections.Generic;
using CrusadersGame.User;
using CrusadersGame;
using System;
using UnityGameEngine.Display;

namespace SkipChestPurchaseConfirm
{
    [BepInPlugin("rathkey.ic.skipchestpurchaseconfirm", "Skip Chest Purchase Confirm", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class SkipChestPurchaseConfirm : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.skipchestpurchaseconfirm");

        void Awake()
        {
            harmony.PatchAll(typeof(SkipChestPurchaseConfirmPatch));
            Debug.Log("SkipChestPurchaseConfirm mod loaded");
        }
    }

    [HarmonyPatch(typeof(ChestItemPurchaseDisplay.ChestPackDisplay), "PurchaseClicked")]
    public static class SkipChestPurchaseConfirmPatch
    {
        static bool Prefix(object __instance)
        {
            var type = __instance.GetType();

            var purchaseButton = (DrawableButton)AccessTools.Field(type, "purchaseButton").GetValue(__instance);
            var packDef = (PremiumItemDef)AccessTools.Field(type, "packDef").GetValue(__instance);
            var offerDef = AccessTools.Field(type, "offerDef").GetValue(__instance);
            var chestDef = (ChestTypeDef)AccessTools.Field(type, "chestDef").GetValue(__instance);

            var softConfirmMethod = AccessTools.Method(type, "SoftConfirm");

            if (!(bool)AccessTools.Property(type, "Selected").GetValue(__instance))
            {
                AccessTools.Method(type, "Clicked").Invoke(__instance, null);
            }

            purchaseButton.Enabled = false;

            if (packDef != null)
            {
                if (offerDef != null)
                {
                    var onOfferStartPurchase = AccessTools.Field(type, "OnOfferStartPurchase").GetValue(__instance);
                    onOfferStartPurchase?.GetType().GetMethod("Invoke").Invoke(onOfferStartPurchase, new object[] { offerDef.GetType().GetProperty("Chests").GetValue(offerDef), packDef.GetOfferChestCount() });

                    UserData.Instance.ChestHandler.BuyChestPack(null, packDef, new Action<bool, List<ChestLootDetails>>((bool success, List<ChestLootDetails> loot) => AccessTools.Method(type, "Purchased").Invoke(__instance, new object[] { success, loot })));
                }
                else
                {
                    var onChestsStartPurchase = AccessTools.Field(type, "OnChestsStartPurchase").GetValue(__instance);
                    onChestsStartPurchase?.GetType().GetMethod("Invoke").Invoke(onChestsStartPurchase, new object[] { chestDef, packDef.GetChestLoot().AddChestAmount, null });

                    UserData.Instance.ChestHandler.BuyChestPack(chestDef, packDef, new Action<bool, List<ChestLootDetails>>((bool success, List<ChestLootDetails> loot) => AccessTools.Method(type, "Purchased").Invoke(__instance, new object[] { success, loot })));
                }
            }
            else if (chestDef != null)
            {
                softConfirmMethod.Invoke(__instance, new object[] { true, null });
            }

            if (Game.ActiveChampionsInstance.Controller.TutorialActive)
            {
                Game.ActiveChampionsInstance.Controller.TutorialController.TutorialEvents.FireSelectChestButton();
            }

            return false;
        }
    }
}