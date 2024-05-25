using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FastFkeys
{
    [BepInPlugin("rathkey.ic.fastfkeys", "Fast Fkeys", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class FastFkeys : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.fastfkeys");

        void Awake()
        {
            harmony.PatchAll(typeof(FastFkeysPatch));
            Debug.Log("FastFkeys mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(HotKeyManager), MethodType.Constructor)]
    public static class FastFkeysPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && ((float)instruction.operand == 0.5f || (float)instruction.operand == 0.1f))
                {
                    instruction.operand = 0.0f;
                }
                yield return instruction;
            }
        }
    }
}