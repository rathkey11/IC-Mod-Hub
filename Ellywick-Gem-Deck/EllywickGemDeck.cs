using HarmonyLib;
using UnityEngine;
using BepInEx;
using System.Collections;
using CrusadersGame.Effects;
using CrusadersGame.GameScreen;

namespace EllywickGemDeck
{
    [BepInPlugin("rathkey.ic.ellywickgemdeck", "Ellywick Gem Deck", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class EllywickGemDeck : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.ellywickgemdeck");

        void Awake()
        {
            harmony.PatchAll();
            Debug.Log("EllywickGemDeck loaded");
        }
    }

    [HarmonyPatch(typeof(EllywickDeckOfManyThingsHandler), "TryDrawCard")]
    public static class GemCardsOnly
    {
        static bool Prefix(object __instance)
        {
            var ultimateActiveField = AccessTools.Field(__instance.GetType(), "ultimateActive");
            var cardDrawTimerField = AccessTools.Field(__instance.GetType(), "cardDrawTimer");
            var currentMonsterKillsField = AccessTools.Field(__instance.GetType(), "currentMonsterKills");
            var cardsInHandField = AccessTools.Field(__instance.GetType(), "cardsInHand");
            var tryAddCardToHandMethod = AccessTools.Method(__instance.GetType(), "TryAddCardToHand");

            bool ultimateActive = (bool)ultimateActiveField.GetValue(__instance);
            if (ultimateActive)
            {
                return false;
            }

            var cardDrawTimer = cardDrawTimerField.GetValue(__instance);
            cardDrawTimer.GetType().GetMethod("Restart").Invoke(cardDrawTimer, null);

            currentMonsterKillsField.SetValue(__instance, 1);

            var cardsInHand = (IList)cardsInHandField.GetValue(__instance);
            while (cardsInHand.Count < 5)
            {
                var cardType = EllywickDeckOfManyThingsHandler.CardType.Gem;
                tryAddCardToHandMethod.Invoke(__instance, new object[] { cardType, true, false });
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(EllywickDeckOfManyThingsHandler), "MonsterKilled")]
    public static class OneKillDeckDraw
    {
        static bool Prefix(object __instance, ActiveCampaignData e, Monster data)
        {
            var tryDrawCardMethod = AccessTools.Method(__instance.GetType(), "TryDrawCard");
            var cardsInHandField = AccessTools.Field(__instance.GetType(), "cardsInHand");

            var cardsInHand = (IList)cardsInHandField.GetValue(__instance);
            while (cardsInHand.Count < 5)
            {
                tryDrawCardMethod.Invoke(__instance, null);
            }

            return false;
        }
    }
}
