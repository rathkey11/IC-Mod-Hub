using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InstantScreenWipe
{
    [BepInPlugin("rathkey.ic.instantscreenwipe", "Instant Screen Wipe", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class InstantScreenWipe : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.instantscreenwipe");

        void Awake()
        {
            harmony.PatchAll(typeof(InstantScreenWipePatch));
            Debug.Log("InstantScreenWipe mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(AreaTransitioner), "OnHeroTransitionOffComplete")]
    public static class InstantScreenWipePatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 2f)
                {
                    instruction.operand = 0f;
                }
                yield return instruction;
            }
        }
    }
}
