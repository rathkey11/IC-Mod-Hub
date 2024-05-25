using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;

namespace GameRunsWhenMinimized
{
    [BepInPlugin("rathkey.ic.gamerunswhenminimized", "Game Runs When Minimized", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class GameRunsWhenMinimized : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.gamerunswhenminimized");

        void Awake()
        {
            harmony.PatchAll(typeof(GameRunsWhenMinimizedPatch));
            Debug.Log("GameRunsWhenMinimized mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(Game), "ApplicationMinimized")]
    public static class GameRunsWhenMinimizedPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_I4_5)
                {
                    codes[i].opcode = OpCodes.Ldc_I4;
                    codes[i].operand = 1000;
                }
            }
            return codes.AsEnumerable();
        }
    }
}