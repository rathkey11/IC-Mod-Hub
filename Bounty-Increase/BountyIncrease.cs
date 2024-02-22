using BepInEx;
using CrusadersGame.Dialogs.Inventory;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BountyIncrease
{
    [BepInPlugin("rathkey.ic.bountyincrease", "Bounty Increase", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    [BepInDependency("rathkey.ic.eventgoobersremoved")]
    public class BountyIncrease : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.bountyincrease");

        void Awake()
        {
            harmony.PatchAll(typeof(BountyIncreasePatch));
            Debug.Log("BountyIncrease mod loaded"); // Log to check if the mod is loading
        }

        [HarmonyPatch(typeof(InventoryItemSelectDialog), "CalculateSlider")]

        public static class BountyIncreasePatch
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                for (int i = 0; i < codes.Count; i++)
                {
                    // Check if the current instruction is loading the value 50 onto the evaluation stack
                    if (codes[i].opcode == OpCodes.Ldc_I4_S && (sbyte)codes[i].operand == 50)
                    {
                        // Replace it with an instruction that loads the value 5000 instead
                        codes[i].opcode = OpCodes.Ldc_I4;
                        codes[i].operand = 5000;
                    }
                }

                return codes;
            }
        }
    }
}
