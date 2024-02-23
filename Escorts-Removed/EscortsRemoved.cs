using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameChanges;
using static CrusadersGame.GameChanges.GameRules;

namespace EscortsRemoved
{
    [BepInPlugin("rathkey.ic.escortsremoved", "Escorts Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class EscortsRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.escortsremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(EscortsRemovedPatch));
            Debug.Log("EscortsRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(GameRules), "get_Escorts")]
    public static class EscortsRemovedPatch
    {
        static bool Prefix(ref GameRules.ChangeByArea<EscortInfo> __result)
        {
            __result = new GameRules.ChangeByArea<EscortInfo>();
            return false;
        }
    }
}
