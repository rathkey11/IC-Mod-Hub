using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen;
using System.Reflection;

namespace StaticAreaTransitions
{
    [BepInPlugin("rathkey.ic.staticareatransitions", "Static Area Transitions", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class StaticAreaTransitions : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.staticareatransitions");

        void Awake()
        {
            harmony.PatchAll(typeof(StaticAreaTransitionsPatch));
            Debug.Log("StaticAreaTransitions mod loaded");
        }
    }

    [HarmonyPatch(typeof(CrusadersGameController), "SetActiveArea")]
    public static class StaticAreaTransitionsPatch
    {
        static bool Prefix(CrusadersGameController __instance, AreaLevel area, bool isUserAction, ref bool instant)
        {
            if (__instance.GameInstance.InOfflineMode)
            {
                instant = true;
            }

            if (!instant)
            {
                __instance.ActiveCampaignData.EventDispatcher.FireAreaChangedTransitionStart(area);
                AreaTransitioner.AreaTransitionDirection direction = AreaTransitioner.AreaTransitionDirection.Static;

                FieldInfo areaTransitionerField = AccessTools.Field(typeof(CrusadersGameController), "areaTransitioner");
                AreaTransitioner areaTransitioner = (AreaTransitioner)areaTransitionerField.GetValue(__instance);

                areaTransitioner.TransitionTo(area, direction, isUserAction);

                return false;
            }

            return true;
        }
    }
}