using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;

namespace HideDamageText
{
    [BepInPlugin("rathkey.ic.hidedamagetext", "Hide Damage Text", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class HideDamageText : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.hidedamagetext");

        void Awake()
        {
            harmony.PatchAll(typeof(HideDamageTextPatch));
            Debug.Log("HideDamageText mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(GameSettings), MethodType.Constructor)]
    public static class HideDamageTextPatch
    {
        static bool Prefix()
        {
            GameSettings.HideDamageText = true;
            return true;
        }
    }
}