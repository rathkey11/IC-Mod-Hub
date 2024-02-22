using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen.VisualEffects;
using System.Reflection;

namespace ParticlesRemoved
{
    [BepInPlugin("rathkey.ic.particlesremoved", "Particles Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class ParticlesRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.particlesremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(ParticlesRemovedPatch));
            Debug.Log("ParticlesRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(ParticleSystem), MethodType.Constructor)]
    public static class ParticlesRemovedPatch
    {
        static void Postfix(ParticleSystem __instance)
        {
            FieldInfo field1 = AccessTools.Field(typeof(ParticleSystem), "maxNewParticlesPerUpdate");
            FieldInfo field2 = AccessTools.Field(typeof(ParticleSystem), "totalMaxNewParticlesPerUpdate");

            if (field1 != null)
            {
                field1.SetValue(__instance, 0);
            }

            if (field2 != null)
            {
                field2.SetValue(null, 0);
            }
        }
    }
}
