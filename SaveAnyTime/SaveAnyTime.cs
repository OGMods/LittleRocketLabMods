using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using MelonLoader;
using System;
using UnityEngine;
using UnityEngine.U2D;

[assembly: MelonInfo(typeof(SaveAnyTime.SaveAnyTimeMod), "Save Any Time", "1.0.0", "OGMods")]
[assembly: MelonGame(null, null)]

namespace SaveAnyTime;

public class SaveAnyTimeMod : MelonMod
{
    // Preference entries
    private static MelonPreferences_Category prefsCategory;
    private static MelonPreferences_Entry<KeyCode> saveKeyEntry;

    // Keycode to be used in update loop
    private KeyCode saveKey;

    public override void OnInitializeMelon()
    {
        // Set up preferences
        prefsCategory = MelonPreferences.CreateCategory("SaveAnyTime", "Save Any Time Settings");
        saveKeyEntry = prefsCategory.CreateEntry("SaveKey", KeyCode.F5, "Save Hotkey", "Key to press for saving the game.");
        MelonPreferences.Save();
        // Load the current key
        saveKey = saveKeyEntry.Value;

        MelonLogger.Msg($"Save Any Time loaded! Press {saveKey} to save.");
    }

    public override void OnUpdate()
    {
        // Reload key in case user changed it at runtime
        if (saveKey != saveKeyEntry.Value)
        {
            saveKey = saveKeyEntry.Value;
            MelonLogger.Msg($"Save key updated to {saveKey}.");
        }

        if (Input.GetKeyDown(saveKey))
        {
            MelonLogger.Msg("Save key pressed! Triggering save...");
            TriggerSave();
        }
    }

    private void TriggerSave()
    {
        try
        {
            // Trigger the actual save
            GameManager.SaveGame(true, new Action(OnSaveComplete));
            MelonLogger.Msg("Save requested...");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"Failed to save game: {ex.Message}");
        }
    }

    private void OnSaveComplete()
    {
        MelonLogger.Msg("Game saved successfully!");
    }
}
