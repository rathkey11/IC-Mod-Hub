using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Dialogs.FormationSave;
using UnityGameEngine.Display;

namespace OldFormationRemoval
{
    [BepInPlugin("rathkey.ic.oldformationremoval", "Old Formation Removal", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class OldFormationRemoval : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.oldformationremoval");

        void Awake()
        {
            harmony.PatchAll(typeof(OldFormationRemovalPatch));
            Debug.Log("OldFormationRemoval mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(FormationSaveDialogItem), "Init")]
    public static class OldFormationRemovalPatch
    {
        static void Prefix(FormationSaveDialogItem __instance)
        {
            var deleteButton = Traverse.Create(__instance).Field("deleteButton");
            deleteButton.SetValue(new DrawableButton("Button_SimpleRed", ""));
        }
    }
}