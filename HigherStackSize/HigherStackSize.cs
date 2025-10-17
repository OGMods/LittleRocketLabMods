using MelonLoader;
using HarmonyLib;
using Il2Cpp;

[assembly: MelonInfo(typeof(HigherStackSize.HigherStackSizeMod), "Higher Stack Size", "1.1.0", "OGMods")]
[assembly: MelonGame(null, null)]

namespace HigherStackSize;

/// <summary>
/// Mod that increases the maximum stack size for stackable items.
/// Supports both fixed stack size mode and multiplier mode.
/// </summary>
public class HigherStackSizeMod : MelonMod
{
    // Preference entries
    private static MelonPreferences_Category prefsCategory;
    private static MelonPreferences_Entry<bool> useMultiplierEntry;
    private static MelonPreferences_Entry<int> stackSizeEntry;
    private static MelonPreferences_Entry<float> multiplierEntry;

    // Values to be used in patches
    private static bool useMultiplier;
    private static int newStackSize;
    private static float multiplier;

    // Track original stack sizes to prevent multiplying already-multiplied values
    private static readonly Dictionary<string, int> originalStackSizes = new();

    public override void OnInitializeMelon()
    {
        // Set up preferences
        prefsCategory = MelonPreferences.CreateCategory("HigherStackSize", "Higher Stack Size Settings");
        prefsCategory.CreateEntry("WARNING", "Lowering stack size or multiplier may cause losing items with existing save files that have larger stacks!", "⚠️ Warning", "Read this warning before changing values!");
        useMultiplierEntry = prefsCategory.CreateEntry("UseMultiplier", false, "Use Multiplier Mode", "If true, multiply the original stack size. If false, set to a fixed value.");
        stackSizeEntry = prefsCategory.CreateEntry("StackSize", 999, "Fixed Stack Size", "The fixed maximum stack size for stackable items when UseMultiplier is false. Default is 999.");
        multiplierEntry = prefsCategory.CreateEntry("Multiplier", 10f, "Stack Size Multiplier", "Multiply the original stack size by this value when UseMultiplier is true. Default is 10x.");
        MelonPreferences.Save();

        // Load the current settings
        useMultiplier = useMultiplierEntry.Value;
        newStackSize = stackSizeEntry.Value;
        multiplier = multiplierEntry.Value;

        if (useMultiplier)
        {
            MelonLogger.Msg($"Higher Stack Size loaded! Using multiplier mode: {multiplier}x original stack size.");
        }
        else
        {
            MelonLogger.Msg($"Higher Stack Size loaded! Using fixed mode: stack size set to {newStackSize}.");
        }

        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        MelonLogger.Error($"Unhandled Exception: {args.ExceptionObject}");
    }

    /// <summary>
    /// Patches an inventory item to modify its maximum stack size based on configuration.
    /// </summary>
    /// <param name="item">The inventory item to patch</param>
    public static void PatchItem(InventoryItem item)
    {
        // Validate item is eligible for patching
        if (item == null || !item.HasQuantity || item.MaxStackSize <= 1)
            return;

        string itemKey = item.Key;
        if (string.IsNullOrEmpty(itemKey))
            return;

        int targetStackSize;

        if (useMultiplier)
        {
            // Store original stack size on first encounter to prevent exponential growth
            if (!originalStackSizes.ContainsKey(itemKey))
            {
                originalStackSizes[itemKey] = item.MaxStackSize;
            }

            int originalStackSize = originalStackSizes[itemKey];

            // Calculate multiplied stack size from original value
            targetStackSize = (int)(originalStackSize * multiplier);
        }
        else
        {
            // Use fixed stack size
            targetStackSize = newStackSize;
        }

        // Apply the new stack size if it differs from current
        if (item.MaxStackSize != targetStackSize)
        {
            item.MaxStackSize = targetStackSize;
        }
    }
}
