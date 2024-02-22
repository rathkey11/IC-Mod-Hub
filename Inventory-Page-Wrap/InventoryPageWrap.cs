using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Dialogs.Inventory;

namespace InventoryPageWrap
{
    [BepInPlugin("rathkey.ic.inventorypagewrap", "Inventory Page Wrap", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class InventoryPageWrap : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.inventorypagewrap");

        void Awake()
        {
            harmony.PatchAll(typeof(InventoryPageWrapPatchLeft));
            harmony.PatchAll(typeof(InventoryPageWrapPatchRight));
            Debug.Log("InventoryPageWrap mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(InventoryPanel), "TurnPageLeft")]
    public static class InventoryPageWrapPatchLeft
    {
        static bool Prefix(InventoryPanel __instance)
        {
            var activePageIndex = (int)AccessTools.Field(typeof(InventoryPanel), "activePageIndex").GetValue(__instance);
            var pages = (int)AccessTools.Field(typeof(InventoryPanel), "pages").GetValue(__instance);

            Debug.Log($"TurnPageLeft - activePageIndex: {activePageIndex}, pages: {pages}"); // Debug log

            if (activePageIndex == 0)
            {
                activePageIndex = pages;
                Debug.Log("TurnPageLeft - activePageIndex set to pages"); // Debug log
            }
            if (activePageIndex > 0)
            {
                int page = activePageIndex - 1;
                activePageIndex = page;
                Debug.Log($"TurnPageLeft - activePageIndex set to {page}"); // Debug log
                AccessTools.Field(typeof(InventoryPanel), "activePageIndex").SetValue(__instance, activePageIndex);
                AccessTools.Method(typeof(InventoryPanel), "LoadPage").Invoke(__instance, new object[] { page });
            }
            return false; // Skip original method
        }
    }

    [HarmonyPatch(typeof(InventoryPanel), "TurnPageRight")]
    public static class InventoryPageWrapPatchRight
    {
        static bool Prefix(InventoryPanel __instance)
        {
            var activePageIndex = (int)AccessTools.Field(typeof(InventoryPanel), "activePageIndex").GetValue(__instance);
            var pages = (int)AccessTools.Field(typeof(InventoryPanel), "pages").GetValue(__instance);

            Debug.Log($"TurnPageRight - activePageIndex: {activePageIndex}, pages: {pages}"); // Debug log

            if (activePageIndex == pages - 1)
            {
                activePageIndex = -1;
                Debug.Log("TurnPageRight - activePageIndex set to -1"); // Debug log
            }
            if (activePageIndex < pages - 1)
            {
                int page = activePageIndex + 1;
                activePageIndex = page;
                Debug.Log($"TurnPageRight - activePageIndex set to {page}"); // Debug log
                AccessTools.Field(typeof(InventoryPanel), "activePageIndex").SetValue(__instance, activePageIndex);
                AccessTools.Method(typeof(InventoryPanel), "LoadPage").Invoke(__instance, new object[] { page });
            }
            return false; // Skip original method
        }
    }
}
