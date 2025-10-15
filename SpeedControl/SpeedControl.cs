using Il2Cpp;
using MelonLoader;
using UnityEngine;
using HarmonyLib;

[assembly: MelonInfo(typeof(SpeedControl.SpeedControlMod), "Speed Control", "1.0.0", "OGMods")]
[assembly: MelonGame(null, null)]

namespace SpeedControl;

public class SpeedControlMod : MelonMod
{
    // Preference entries
    private static MelonPreferences_Category prefsCategory;
    private static MelonPreferences_Entry<KeyCode> increaseKeyEntry;
    private static MelonPreferences_Entry<KeyCode> decreaseKeyEntry;
    private static MelonPreferences_Entry<KeyCode> resetKeyEntry;

    // Keycodes to be used in update loop
    private KeyCode increaseKey;
    private KeyCode decreaseKey;
    private KeyCode resetKey;

    // Speed Control settings
    private const float MIN_SPEED = 0.5f;
    private const float MAX_SPEED = 10f;
    private const float SPEED_INCREMENT = 0.5f;
    private const float DEFAULT_SPEED = 1f;

    private float currentSpeed = DEFAULT_SPEED;

    public override void OnInitializeMelon()
    {
        // Apply Harmony patches
        MelonLogger.Msg("Applying Harmony patches...");
        var harmony = HarmonyInstance;
        harmony.PatchAll(typeof(SpeedControlMod).Assembly);
        MelonLogger.Msg("Harmony patches applied!");

        // Set up preferences
        prefsCategory = MelonPreferences.CreateCategory("SpeedControl", "Speed Control Settings");
        increaseKeyEntry = prefsCategory.CreateEntry("IncreaseKey", KeyCode.KeypadPlus, "Increase Speed Control", "Key to increase Speed Control.");
        decreaseKeyEntry = prefsCategory.CreateEntry("DecreaseKey", KeyCode.KeypadMinus, "Decrease Speed Control", "Key to decrease Speed Control.");
        resetKeyEntry = prefsCategory.CreateEntry("ResetKey", KeyCode.KeypadMultiply, "Reset Speed Control", "Key to reset Speed Control to 1.0.");
        MelonPreferences.Save();

        // Load the current keys
        increaseKey = increaseKeyEntry.Value;
        decreaseKey = decreaseKeyEntry.Value;
        resetKey = resetKeyEntry.Value;

        MelonLogger.Msg($"Speed Control loaded!");
        MelonLogger.Msg($"  Increase: {increaseKey}");
        MelonLogger.Msg($"  Decrease: {decreaseKey}");
        MelonLogger.Msg($"  Reset: {resetKey}");
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        // Reapply the current speed when a scene loads (Unity resets Time.timeScale to 1.0)
        if (currentSpeed != DEFAULT_SPEED)
        {
            ApplySpeed();
            MelonLogger.Msg($"Scene '{sceneName}' loaded - reapplied speed to {currentSpeed:F1}x");
        }
        else
        {
            MelonLogger.Msg($"Scene0 '{sceneName}' loaded - reapplied speed to {currentSpeed:F1}x");
        }
    }

    public override void OnUpdate()
    {
        // Reload keys in case user changed them at runtime
        if (increaseKey != increaseKeyEntry.Value)
        {
            increaseKey = increaseKeyEntry.Value;
            MelonLogger.Msg($"Increase key updated to {increaseKey}.");
        }
        if (decreaseKey != decreaseKeyEntry.Value)
        {
            decreaseKey = decreaseKeyEntry.Value;
            MelonLogger.Msg($"Decrease key updated to {decreaseKey}.");
        }
        if (resetKey != resetKeyEntry.Value)
        {
            resetKey = resetKeyEntry.Value;
            MelonLogger.Msg($"Reset key updated to {resetKey}.");
        }

        // Check for key presses
        if (Input.GetKeyDown(increaseKey))
        {
            IncreaseSpeed();
        }
        else if (Input.GetKeyDown(decreaseKey))
        {
            DecreaseSpeed();
        }
        else if (Input.GetKeyDown(resetKey))
        {
            ResetSpeed();
        }

        if (currentSpeed != Time.timeScale)
        {
            ApplySpeed();
        }
    }

    private void IncreaseSpeed()
    {
        currentSpeed += SPEED_INCREMENT;
        if (currentSpeed > MAX_SPEED)
        {
            currentSpeed = MAX_SPEED;
        }
        ApplySpeed();
        ShowNotification();
    }

    private void DecreaseSpeed()
    {
        currentSpeed -= SPEED_INCREMENT;
        if (currentSpeed < MIN_SPEED)
        {
            currentSpeed = MIN_SPEED;
        }
        ApplySpeed();
        ShowNotification();
    }

    private void ResetSpeed()
    {
        currentSpeed = DEFAULT_SPEED;
        ApplySpeed();
        ShowNotification();
    }

    private void ApplySpeed()
    {
        Time.timeScale = currentSpeed;
        MelonLogger.Msg($"Speed Control set to {currentSpeed:F1}x");
    }

    private void ShowNotification()
    {
        try
        {
            var notificationCoordinator = FindComponent<NotificationCoordinator>("NotificationCoordinator");
            if (notificationCoordinator != null)
            {
                // Show the notification with the Speed Control value as argument
                notificationCoordinator.Show("SpeedChanged", $"{currentSpeed:F1}x");
                MelonLogger.Msg($"Showing Speed Control notification: {currentSpeed:F1}x");
            }
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error($"Failed to show notification: {ex.Message}");
        }
    }

    private static T FindComponent<T>(string gameObjectName) where T : Component
    {
        try
        {
            GameObject gameObject = GameObject.Find(gameObjectName);
            if (gameObject == null)
            {
                return null;
            }

            T component = gameObject.GetComponent<T>();
            return component;
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error($"Error finding {typeof(T).Name} on {gameObjectName}: {ex.Message}");
            return null;
        }
    }
}
