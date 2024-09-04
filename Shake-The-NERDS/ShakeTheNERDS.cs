using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.Effects;
using System.Collections;

namespace ShakeTheNERDS
{
    [BepInPlugin("rathkey.ic.shakethenerds", "Shake The NERDS", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class ShakeTheNERDS : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.shakethenerds");

        void Awake()
        {
            harmony.PatchAll(typeof(ShakeTheNERDSPatch));
            Debug.Log("ShakeTheNERDS mod loaded");
        }
    }

    [HarmonyPatch(typeof(NerdWagonHandler), "ShakeTheBox")]
    public static class ShakeTheNERDSPatch
    {
        static bool Prefix(NerdWagonHandler __instance)
        {
            var availableNerdsField = AccessTools.Field(typeof(NerdWagonHandler), "availableNerds");
            var nerd0Field = AccessTools.Field(typeof(NerdWagonHandler), "nerd0");
            var nerd1Field = AccessTools.Field(typeof(NerdWagonHandler), "nerd1");
            var nerd2Field = AccessTools.Field(typeof(NerdWagonHandler), "nerd2");

            var availableNerds = (IList)availableNerdsField.GetValue(__instance);

            availableNerds.Clear();

            var getNerdByTypeMethod = AccessTools.Method(typeof(NerdWagonHandler), "GetNerdByType");

            var nerd0 = getNerdByTypeMethod.Invoke(__instance, new object[] { NerdWagonHandler.NerdType.Fighter_Orange });
            var nerd1 = getNerdByTypeMethod.Invoke(__instance, new object[] { NerdWagonHandler.NerdType.Ranger_Red });
            var nerd2 = getNerdByTypeMethod.Invoke(__instance, new object[] { NerdWagonHandler.NerdType.Bard_Green });

            nerd0Field.SetValue(__instance, nerd0);
            nerd1Field.SetValue(__instance, nerd1);
            nerd2Field.SetValue(__instance, nerd2);

            var nerd0TypeField = AccessTools.Property(typeof(NerdWagonHandler), "nerd0Type");
            var nerd1TypeField = AccessTools.Property(typeof(NerdWagonHandler), "nerd1Type");
            var nerd2TypeField = AccessTools.Property(typeof(NerdWagonHandler), "nerd2Type");

            nerd0TypeField.SetValue(__instance, NerdWagonHandler.NerdType.Fighter_Orange);
            nerd1TypeField.SetValue(__instance, NerdWagonHandler.NerdType.Ranger_Red);
            nerd2TypeField.SetValue(__instance, NerdWagonHandler.NerdType.Bard_Green);

            var syncNerdGraphicsMethod = AccessTools.Method(typeof(NerdWagonHandler), "SyncNerdGraphics");
            syncNerdGraphicsMethod.Invoke(__instance, null);

            var onActiveNerdsChangedField = AccessTools.Field(typeof(NerdWagonHandler), "OnActiveNerdsChanged");
            var onActiveNerdsChanged = (System.Action<NerdWagonHandler>)onActiveNerdsChangedField.GetValue(null);
            if (onActiveNerdsChanged != null)
            {
                onActiveNerdsChanged(__instance);
            }

            return false;
        }
    }
}