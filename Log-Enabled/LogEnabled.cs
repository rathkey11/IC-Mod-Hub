using HarmonyLib;
using UnityEngine;
using BepInEx;

namespace LogEnabled
{
    [BepInPlugin("rathkey.ic.logenabled", "Log Enabled", "0.1.0")]
    [BepInProcess("IdleDragons.exe")]
    public class LogEnabled : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("rathkey.ic.logenabled");

        void Awake()
        {
            harmony.PatchAll(typeof(LogEnabledPatch));
            Debug.Log("LogEnabled mod loaded"); // Log to check if the mod is loading
        }
    }

    [HarmonyPatch(typeof(IdleGameManager), "StartGame")]
    public static class LogEnabledPatch
    {
        static void Postfix()
        {
            var unityLoggerProperty = AccessTools.Property(typeof(Debug), "unityLogger");
            var unityLogger = (ILogger)unityLoggerProperty.GetValue(null, null);
            unityLogger.logEnabled = true;
        }
    }
}
