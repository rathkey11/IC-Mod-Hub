using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame;
using CrusadersGame.Dialogs;
using System;

namespace FramerateIncrease
{
    [BepInPlugin("rathkey.ic.framerateincrease", "Framerate Increase", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class FramerateIncrease : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.framerateincrease");

        void Awake()
        {
            harmony.PatchAll();
            Debug.Log("FramerateIncrease mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(GameSettings), MethodType.Constructor)]
    class FramerateIncreasePatch
    {
        static void Postfix()
        {
            GameSettings.TargetFramerate = 1000;
        }
    }

    [HarmonyPatch(typeof(TitledSlider), "Init", new Type[] { typeof(string), typeof(string), typeof(int), typeof(int), typeof(int), typeof(string) })]
    class SliderPatch1
    {
        static void Prefix(ref int maxValue)
        {
            maxValue = 1000;
        }
    }

    [HarmonyPatch(typeof(Slider), "Init", new Type[] { typeof(int), typeof(int), typeof(int), typeof(string), typeof(bool) })]
    class SliderPatch2
    {
        static void Prefix(ref int maxValue)
        {
            maxValue = 1000;
        }
    }
}
