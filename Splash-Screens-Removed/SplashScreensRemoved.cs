using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;

namespace SplashScreensRemoved
{
    [BepInPlugin("rathkey.ic.splashscreensremoved", "Splash Screens Removed", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class SplashScreensRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.splashscreensremoved");

        void Awake()
        {
            harmony.PatchAll(typeof(SplashScreensRemovedPatch));
            Debug.Log("SplashScreensRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(LoadingScreen), "ShowSplashScreens")]
    public static class SplashScreensRemovedPatch
    {
        static bool Prefix()
        {
            return false;
        }
    }
}