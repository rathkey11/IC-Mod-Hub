using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Actions;
using CrusadersGame.GameScreen;
using CrusadersGame.SpecialEvent;
using Engine.Numeric;
using System.Reflection;

namespace EventGoobersRemoved
{
    [BepInPlugin("rathkey.ic.eventgoobersremoved", "Event Goobers Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class EventGoobersRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.eventgoobersremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(EventGoobersRemovedPatch));
            Debug.Log("EventGoobersRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(SecondsWorthOfPlayAction), "Execute")]
    class EventGoobersRemovedPatch
    {
        static bool Prefix(SecondsWorthOfPlayAction __instance, CrusadersGameController controller, bool isUserActionEffect)
        {
            var goldField = typeof(SecondsWorthOfPlayAction).GetField("gold", BindingFlags.NonPublic | BindingFlags.Instance);
            var secondsField = typeof(SecondsWorthOfPlayAction).GetField("seconds", BindingFlags.NonPublic | BindingFlags.Instance);
            var cooldownField = typeof(SecondsWorthOfPlayAction).GetField("cooldown", BindingFlags.NonPublic | BindingFlags.Instance);
            var eventTokensField = typeof(SecondsWorthOfPlayAction).GetField("eventTokens", BindingFlags.NonPublic | BindingFlags.Instance);

            bool gold = (bool)goldField.GetValue(__instance);
            int seconds = (int)secondsField.GetValue(__instance);
            bool cooldown = (bool)cooldownField.GetValue(__instance);
            bool eventTokens = (bool)eventTokensField.GetValue(__instance);

            if (gold)
            {
                Quad value = controller.ActiveCampaignData.CalculateIdleGoldRateModified(false, null) * seconds;
                controller.ActiveCampaignData.SanityCheckGoldRateFromInstrumentedStats(value, (double)seconds, false);
            }
            bool flag = cooldown;
            if (eventTokens)
            {
                SpecialEventHandler activeEvent = controller.UserData.SpecialEvents.GetActiveEvent();
                if (activeEvent != null && activeEvent.EventToken != null)
                {
                    int num = activeEvent.CalculateSecondsWorthOfTokens(controller.UserData, seconds);
                }
            }
            controller.ActiveCampaignData.EventDispatcher.FireSecondsWorthOfPlay(seconds);
            return false;
        }
    }

}
