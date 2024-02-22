using HarmonyLib;
using UnityEngine;
using BepInEx;
using UnityGameEngine.Utilities;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace GameDialogSpeedIncrease
{
    [BepInPlugin("rathkey.ic.gamedialogspeedincrease", "Game Dialog Speed Increase", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class GameDialogSpeedIncrease : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.gamedialogspeedincrease");

        void Awake()
        {
            harmony.PatchAll(typeof(GameDialogSpeedIncreasePatch));
            harmony.PatchAll(typeof(GameDialogSpeedIncreasePatch2));
            Debug.Log("GameDialogSpeedIncrease mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(SimpleTween), MethodType.Constructor)]
    public static class GameDialogSpeedIncreasePatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var inst in instructions)
            {
                if (inst.opcode == OpCodes.Ldc_R4 && (float)inst.operand == 1f)
                {
                    inst.operand = 10f;
                }
                yield return inst;
            }
        }
    }

    [HarmonyPatch(typeof(SimpleTweenDouble), MethodType.Constructor)]
    public static class GameDialogSpeedIncreasePatch2
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var inst in instructions)
            {
                if (inst.opcode == OpCodes.Ldc_R4 && (float)inst.operand == 1f)
                {
                    inst.operand = 10f;
                }
                yield return inst;
            }
        }
    }
}
