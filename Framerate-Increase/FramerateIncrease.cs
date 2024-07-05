using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using System;
using CrusadersGame.Dialogs.Settings;
using CrusadersGame.GameScreen;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FramerateIncrease
{
    [BepInPlugin("rathkey.ic.framerateincrease", "Framerate Increase", "0.2.0")]
    [BepInProcess("IdleDragons.exe")]
    public class FramerateIncrease : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.framerateincrease");

        void Awake()
        {
            harmony.PatchAll();
            Debug.Log("FramerateIncrease mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(GameSettings), MethodType.Constructor)]
    class FramerateIncreasePatch
    {
        static void Postfix()
        {
            GameSettings.TargetFramerate = 1000;
        }
    }

    [HarmonyPatch(typeof(SettingsPanel), MethodType.Constructor, new Type[] { typeof(CrusadersGameController) })]
    class SettingsPanelTranspiler
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_I4_S && (sbyte)instruction.operand == 60)
                {
                    instruction.opcode = OpCodes.Ldc_I4;
                    instruction.operand = 1000;
                }
                yield return instruction;
            }
        }
    }

    [HarmonyPatch(typeof(SettingsPanel2GraphicsPanel), MethodType.Constructor, new Type[] { typeof(CrusadersGameController) })]
    class SettingsPanel2GraphicsPanelTranspiler
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_I4_S && (sbyte)instruction.operand == 60)
                {
                    instruction.opcode = OpCodes.Ldc_I4;
                    instruction.operand = 1000;
                }
                yield return instruction;
            }
        }
    }
}
