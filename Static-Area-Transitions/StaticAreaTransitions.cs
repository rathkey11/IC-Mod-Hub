using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StaticAreaTransitions
{
    [BepInPlugin("rathkey.ic.staticareatransitions", "Static Area Transitions", "0.2.0")]
    [BepInProcess("IdleDragons.exe")]
    public class StaticAreaTransitions : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.staticareatransitions");

        void Awake()
        {
            harmony.PatchAll(typeof(StaticAreaTransitionsPatch));
            Debug.Log("StaticAreaTransitions mod loaded");
        }
    }

    [HarmonyPatch(typeof(CrusadersGameController), "SetActiveArea")]
    public static class StaticAreaTransitionsPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                // Look for forward or backward direction
                if (codes[i].opcode == OpCodes.Ldc_I4_0 || codes[i].opcode == OpCodes.Ldc_I4_1)
                {
                    // Replace with static direction
                    codes[i] = new CodeInstruction(OpCodes.Ldc_I4_2);
                }
            }
            return codes;
        }
    }
}
