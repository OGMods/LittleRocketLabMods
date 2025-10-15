using MelonLoader;
using HarmonyLib;
using Il2Cpp;

[assembly: MelonInfo(typeof(HigherStackSize.HigherStackSizeMod), "Higher Stack Size", "1.0.0", "OGMods")]
[assembly: MelonGame(null, null)]

namespace HigherStackSize;

public class HigherStackSizeMod : MelonMod
{
    private const int NEW_STACK_SIZE = 999;
    private static readonly HashSet<string> patchedItems = new();

    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("Higher Stack Size loaded!");
    }

    public static void PatchItem(InventoryItem item)
    {
        if (item != null && item.HasQuantity && item.MaxStackSize != NEW_STACK_SIZE)
        {
            item.MaxStackSize = NEW_STACK_SIZE;

            if (!patchedItems.Contains(item.Key))
            {
                patchedItems.Add(item.Key);
                MelonLogger.Msg($"Patched {item.Key}: MaxStackSize = {NEW_STACK_SIZE}");
            }
        }
    }
}

[HarmonyPatch(typeof(InventorySlot), "Load")]
public class InventorySlot_Load_Patch
{
    [HarmonyPrefix]
    public static void Prefix(InventorySlotSaveData saveData)
    {
        if (saveData == null || string.IsNullOrEmpty(saveData.Key)) return;

        try
        {
            var repository = GameManager.Repository;
            if (repository?.Items == null) return;

            var item = repository.Items.Get(saveData.Key, false);
            HigherStackSizeMod.PatchItem(item);
        }
        catch (System.Exception e)
        {
            MelonLogger.Error($"Error in Load Prefix: {e.Message}");
        }
    }

    [HarmonyPostfix]
    public static void Postfix(InventorySlot __instance)
    {
        HigherStackSizeMod.PatchItem(__instance?.Item);
        HigherStackSizeMod.PatchItem(__instance?.RequiredItem);
        HigherStackSizeMod.PatchItem(__instance?.LockedItem);
    }
}

[HarmonyPatch(typeof(InventorySlot), nameof(InventorySlot.AddItem))]
public class InventorySlot_AddItem_Patch
{
    [HarmonyPrefix]
    public static void Prefix(InventorySlot __instance, InventoryItem item)
    {
        HigherStackSizeMod.PatchItem(item);
        HigherStackSizeMod.PatchItem(__instance?.Item);
        HigherStackSizeMod.PatchItem(__instance?.RequiredItem);
        HigherStackSizeMod.PatchItem(__instance?.LockedItem);
    }
}

[HarmonyPatch(typeof(InventoryData), "Sort")]
public class InventoryData_Sort_Patch
{
    [HarmonyPrefix]
    public static void Prefix(InventoryData __instance)
    {
        if (__instance?._inventorySlots == null) return;

        foreach (var slot in __instance._inventorySlots)
        {
            if (slot == null) continue;

            HigherStackSizeMod.PatchItem(slot.Item);
            HigherStackSizeMod.PatchItem(slot.LockedItem);
            HigherStackSizeMod.PatchItem(slot.RequiredItem);
        }
    }
}

[HarmonyPatch(typeof(InventorySlot), "IsFull")]
public class InventorySlot_IsFull_Patch
{
    [HarmonyPrefix]
    public static void Prefix(InventorySlot __instance)
    {
        HigherStackSizeMod.PatchItem(__instance?.Item);
    }
}

[HarmonyPatch(typeof(InventorySlot), nameof(InventorySlot.CanAddItem))]
public class InventorySlot_CanAddItem_Patch
{
    [HarmonyPrefix]
    public static void Prefix(InventorySlot __instance, InventoryItem item)
    {
        HigherStackSizeMod.PatchItem(item);
        HigherStackSizeMod.PatchItem(__instance?.Item);
        HigherStackSizeMod.PatchItem(__instance?.RequiredItem);
        HigherStackSizeMod.PatchItem(__instance?.LockedItem);
    }
}

[HarmonyPatch(typeof(InventorySlot), "TestAddItem")]
public class InventorySlot_TestAddItem_Patch
{
    [HarmonyPrefix]
    public static void Prefix(InventorySlot __instance, InventoryItem item)
    {
        HigherStackSizeMod.PatchItem(item);
        HigherStackSizeMod.PatchItem(__instance?.Item);
        HigherStackSizeMod.PatchItem(__instance?.RequiredItem);
        HigherStackSizeMod.PatchItem(__instance?.LockedItem);
    }
}

[HarmonyPatch(typeof(CachedAssetDatabase<InventoryItem>), "Get", new Type[] { typeof(string), typeof(bool) })]
public class CachedAssetDatabase_Get_Patch
{
    [HarmonyPostfix]
    public static void Postfix(InventoryItem __result)
    {
        HigherStackSizeMod.PatchItem(__result);
    }
}