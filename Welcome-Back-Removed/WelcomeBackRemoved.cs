using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen.UIComponents;

namespace WelcomeBackRemoved
{
    [BepInPlugin("rathkey.ic.welcomebackremoved", "Welcome Back Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class WelcomeBackRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.welcomebackremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(WelcomeBackRemovedPatch));
            Debug.Log("WelcomeBackRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(WelcomeBackNotification), MethodType.Constructor)]
    public static class WelcomeBackRemovedPatch
    {
        static bool Prefix()
        {
            return false;
        }
    }
}