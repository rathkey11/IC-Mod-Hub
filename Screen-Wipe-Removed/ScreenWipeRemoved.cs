using System;
using HarmonyLib;
using UnityEngine;
using BepInEx;
using CrusadersGame.GameScreen;
using CrusadersGame;
using UnityGameEngine.Display;
using UnityGameEngine.Utilities;

namespace ScreenWipeRemoved
{
    [BepInPlugin("rathkey.ic.screenwiperemoved", "Screen Wipe Removed", "0.2.0")]
    [BepInProcess("IdleDragons.exe")]
    public class ScreenWipeRemoved : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.screenwiperemoved");

        void Awake()
        {
            harmony.PatchAll(typeof(ScreenWipeRemovedPatch));
            Debug.Log("ScreenWipeRemoved mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(ScreenWipeEffect), "Start")]
    public static class ScreenWipeRemovedPatch
    {
        static bool Prefix(ScreenWipeEffect __instance, float duration, Action onHalfwayCallback, Action onCompleteCallback)
        {
            Debug.Log("ScreenWipeRemovedPatch: Start method called");

            // Get private fields using AccessTools
            var wipeHalfwayCallback = AccessTools.Field(typeof(ScreenWipeEffect), "wipeHalfwayCallback");
            var wipeCompleteCallback = AccessTools.Field(typeof(ScreenWipeEffect), "wipeCompleteCallback");
            var wipeBox = AccessTools.Field(typeof(ScreenWipeEffect), "wipeBox");
            var wipeBoxColor = AccessTools.Field(typeof(ScreenWipeEffect), "wipeBoxColor");
            var time = AccessTools.Field(typeof(ScreenWipeEffect), "time");
            var wipeTween = AccessTools.Field(typeof(ScreenWipeEffect), "wipeTween");

            Debug.Log("ScreenWipeRemovedPatch: Private fields accessed");

            // Set private fields
            wipeHalfwayCallback.SetValue(__instance, onHalfwayCallback);
            wipeCompleteCallback.SetValue(__instance, onCompleteCallback);

            // Get the wipeBox object and cast it to Drawable
            var wipeBoxInstance = (Drawable)wipeBox.GetValue(__instance);
            wipeBoxInstance.MouseEnabled = false;
            wipeBoxInstance.MouseChildren = false;
            wipeBoxInstance.GetSprite().SetAsBox(GameSettings.ScreenWidth, GameSettings.ScreenHeight, new Color?((Color)wipeBoxColor.GetValue(__instance)), new Color?((Color)wipeBoxColor.GetValue(__instance)), false);
            wipeBoxInstance.X = (float)(-(float)GameSettings.ScreenWidth);

            Debug.Log("ScreenWipeRemovedPatch: wipeBox properties set");

            // Calculate end and time
            float end = (float)((onHalfwayCallback != null) ? 0 : GameSettings.ScreenWidth);
            time.SetValue(__instance, ((onHalfwayCallback != null) ? (duration * 0.5f) : duration));

            // Get the wipeTween object and cast it to SimpleTween
            var wipeTweenInstance = (SimpleTween)wipeTween.GetValue(__instance);
            wipeTweenInstance.Init(delegate (float x)
            {
                wipeBoxInstance.X = x;
            }, SimpleTween.EaseLinear, wipeBoxInstance.X, end, (float)time.GetValue(__instance), true);

            Debug.Log("ScreenWipeRemovedPatch: wipeTween properties set");

            if (onHalfwayCallback != null)
            {
                wipeTweenInstance.OnFinish = new SimpleTweenFinish(__instance.OnWipeHalfway);
                Debug.Log("ScreenWipeRemovedPatch: OnHalfwayCallback is not null");
                return false;
            }
            wipeTweenInstance.OnFinish = new SimpleTweenFinish(__instance.OnWipeComplete);
            Debug.Log("ScreenWipeRemovedPatch: OnCompleteCallback is set");
            return false;
        }
    }
}