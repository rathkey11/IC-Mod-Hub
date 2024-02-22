using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;

namespace AdminModeEnabled
{
    [BepInPlugin("rathkey.ic.adminmodeenabled", "Admin Mode Enabled", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class AdminModeEnabled : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.adminmodeenabled");

        void Awake()
        {
            harmony.PatchAll(typeof(AdminModeEnabledPatch));
            Debug.Log("AdminModeEnabled mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(GameSettings), "ADMIN", MethodType.Getter)]
    public static class AdminModeEnabledPatch
    {
        static bool Prefix(GameSettings __instance, ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
