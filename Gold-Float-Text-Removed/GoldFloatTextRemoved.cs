using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen.Goobers;

namespace GoldFloatTextRemoved
{
    [BepInPlugin("rathkey.ic.goldfloattextremoved", "Gold Float Text Removed", "0.2.0")]
    [BepInProcess("IdleDragons.exe")]
    public class GoldFloatTextRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.goldfloattextremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(GoldFloatTextRemovedPatch));
            Debug.Log("GoldFloatTextRemoved mod loaded");
        }
    }

    [HarmonyPatch(typeof(Goober), "PickupAfterDelay")]
    public static class GoldFloatTextRemovedPatch
    {
        static void Prefix(object __instance)
        {
            var gooberDataField = AccessTools.Field(typeof(Goober), "gooberData");
            var gooberData = (Goober.GooberData)gooberDataField.GetValue(__instance);

            var numTextDrawablesField = AccessTools.Field(typeof(Goober), "numTextDrawables");
            var numTextDrawables = (int)numTextDrawablesField.GetValue(null);

            bool flag = true;
            if (numTextDrawables >= 50)
            {
                flag = (numTextDrawables % (numTextDrawables / 25) == 0);
            }

            if (flag && gooberData.Payload.Type == GooberPayload.PayloadType.Gold)
            {
                gooberData.Text = string.Empty;
            }
        }
    }
}