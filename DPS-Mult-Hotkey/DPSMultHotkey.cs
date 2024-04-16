using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using CrusadersGame.Defs;
using CrusadersGame.Effects;
using CrusadersGame.GameScreen;

namespace DPSMultHotkey
{
    [BepInPlugin("rathkey.ic.dpsmulthotkey", "DPS Mult Hotkey", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class DPSMultHotkey : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.dpsmulthotkey");

        void Awake()
        {
            harmony.PatchAll(typeof(DPSMultHotkeyPatch));
            Debug.Log("DPSMultHotkey mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(HotKeyManager), "CheckForDialog")]
    public static class DPSMultHotkeyPatch
    {
        static bool Prefix(HotKeyManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                var controller = AccessTools.Field(typeof(HotKeyManager), "controller").GetValue(__instance) as CrusadersGameController;
                if (controller != null && controller.ActiveCampaignData.CurrentRules.ForceResetArea == null)
                {
                    Effect effect = new Effect(controller, new SimpleEffectSource("TEST"), EffectDef.GetEffectDefFromString("global_dps_multiplier_mult,1.0e308", null), controller.ActiveCampaignData, false);
                    effect.BaseEffectKey.SetEffectKeyTime(100000.0);
                    effect.CheckRequirementsAndApply();
                    return false; // Skip the original method only when R key is pressed and ForceResetArea is null
                }
            }
            return true; // Continue with the original method in all other cases
        }
    }

}