using HarmonyLib;
using UnityEngine;
using BepInEx;
using System.Collections.Generic;
using System.Reflection.Emit;
using CrusadersGame.GameScreen.Familiars;

namespace FamiliarPickups
{
    [BepInPlugin("rathkey.ic.familiarpickups", "Familiar Pickups", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class FamiliarPickups : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.familiarpickups");

        void Awake()
        {
            harmony.PatchAll(typeof(FamiliarPickupsPatch));
            Debug.Log("FamiliarPickups mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(FamiliarSlot), "Update")]
    public static class FamiliarPickupsPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if ((instruction.opcode == OpCodes.Ldc_I4_3 || instruction.opcode == OpCodes.Ldc_I4_5 || instruction.opcode == OpCodes.Ldc_I4_6))
                {
                    instruction.opcode = OpCodes.Ldc_I4_1;
                }

                yield return instruction;
            }
        }
    }
}