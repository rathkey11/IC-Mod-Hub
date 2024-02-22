using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace TimeScaleMaxed
{
    [BepInPlugin("rathkey.ic.timescalemaxed", "TimeScale Maxed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class TimeScaleMaxed : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.timescalemaxed");

        void Awake()
        {
            harmony.PatchAll(typeof(TimeScaleMaxedPatch));
            Debug.Log("TimeScaleMaxed mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(ChampionsGameInstance), "RecalculateTimeScale")]
    public static class TimeScaleMaxedPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 1f)
                {
                    instruction.operand = 10f;
                }

                yield return instruction;
            }
        }
    }
}
