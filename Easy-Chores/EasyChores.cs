using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.User;
using CrusadersGame;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EasyChores
{
    [BepInPlugin("rathkey.ic.easychores", "Easy Chores", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class EasyChores : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.easychores");

        void Awake()
        {
            harmony.PatchAll();
            Debug.Log("EasyChores mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(UserChallengeHandler.ChallengeSetStats), "UpdateStatValue")]
    public static class EasyChoresPatch1
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R8 && (double)instruction.operand == 0.20000000298023224)
                {
                    instruction.operand = 1.0;
                }

                yield return instruction;
            }
        }
    }
    [HarmonyPatch(typeof(GameSettings), MethodType.Constructor)]
    public static class EasyChoresPatch2
    {
        public static void Postfix()
        {
            GameSettings.ChallengeStatAdminMod = true;
        }
    }
}
