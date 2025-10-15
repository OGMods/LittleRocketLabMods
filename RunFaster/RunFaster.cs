using MelonLoader;
using HarmonyLib;
using Il2Cpp;

[assembly: MelonInfo(typeof(RunFaster.RunFasterMod), "Run Faster", "1.0.0", "OGMods")]
[assembly: MelonGame(null, null)]

namespace RunFaster;

public class RunFasterMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("Run Faster mod loaded!");
    }
}

[HarmonyPatch(typeof(DebugOptions), "Awake")]
public class DebugOptions_Awake_Patch
{
    [HarmonyPostfix]
    public static void Postfix(DebugOptions __instance)
    {
        // Enable run speed boost
        __instance.RunReallyFast = true;

        MelonLogger.Msg("Run speed enabled!");
    }
}

[HarmonyPatch(typeof(DebugOptions), "Update")]
public class DebugOptions_Update_Patch
{
    [HarmonyPrefix]
    public static void Prefix(DebugOptions __instance)
    {
        // Ensure run speed stays enabled
        if (!__instance.RunReallyFast)
        {
            __instance.RunReallyFast = true;
        }
    }
}
