using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

[assembly: MelonInfo(typeof(ScrollSplit.ScrollSplitMod), "ScrollSplit", "1.0.0", "OGMods")]
[assembly: MelonGame(null, null)]

namespace ScrollSplit;

/// <summary>
/// ScrollSplit mod allows players to split item stacks using the mouse scroll wheel.
/// Scroll up to pick up items one at a time, scroll down to place them back.
/// </summary>
public class ScrollSplitMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("ScrollSplit mod loaded!");
    }
}

[HarmonyPatch(typeof(InventoryController))]
public class InventoryControllerPatch
{
    private const float SOUND_COOLDOWN = 0.3f;
    private static float lastSoundTime = -SOUND_COOLDOWN;

    /// <summary>
    /// Patches the InventoryController Update method to add scroll-based item splitting.
    /// </summary>
    [HarmonyPatch("Update")]
    [HarmonyPostfix]
    public static void UpdatePostfix(InventoryController __instance)
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollDelta) < 0.01f)
            return;

        InventorySlotController hoveredSlot = __instance._hoveredSlot;
        InventorySlotController ghostSlot = __instance.GhostSlot;
        if (hoveredSlot == null)
            return;

        if (scrollDelta > 0)
        {
            HandleScrollUp(__instance);
        }
        else if (scrollDelta < 0)
        {
            HandleScrollDown(hoveredSlot, ghostSlot);
        }
    }

    /// <summary>
    /// Handles scrolling up to pick up items from the hovered slot.
    /// </summary>
    private static void HandleScrollUp(InventoryController instance)
    {
        InventorySlotController hoveredSlot = instance._hoveredSlot;
        InventorySlotController ghostSlot = instance.GhostSlot;
        if (!hoveredSlot.HasItem)
            return;

        InventoryItem hoveredItem = hoveredSlot.Item;
        if (hoveredItem == null || !hoveredItem.HasQuantity)
            return;

        int currentQuantity = hoveredSlot.Quantity;
        if (currentQuantity <= 1)
            return;

        // Prevent mixing different item types
        if (ghostSlot != null && ghostSlot.HasItem)
        {
            InventoryItem ghostItem = ghostSlot.Item;
            if (ghostItem != null && ghostItem.Key != hoveredItem.Key)
                return;
        }

        // Pick up one item from the hovered slot
        if (ghostSlot == null || !ghostSlot.HasItem)
        {
            instance.SelectSlot(hoveredSlot);
            ghostSlot.MoveTo(hoveredSlot, currentQuantity - 1);
        } else
        {
            hoveredSlot.MoveTo(ghostSlot, 1);
        }

        PlaySoundWithCooldown("event:/SFX/Inventory/4_13_SplitStack");
    }

    /// <summary>
    /// Handles scrolling down to place items into the hovered slot.
    /// </summary>
    private static void HandleScrollDown(InventorySlotController hoveredSlot, InventorySlotController ghostSlot)
    {
        if (ghostSlot == null || !ghostSlot.HasItem)
            return;

        int ghostQuantity = ghostSlot.Quantity;
        if (ghostQuantity <= 1)
            return;

        // Prevent mixing different item types
        if (hoveredSlot.HasItem)
        {
            InventoryItem hoveredItem = hoveredSlot.Item;
            InventoryItem ghostItem = ghostSlot.Item;
            if (hoveredItem != null && ghostItem != null && hoveredItem.Key != ghostItem.Key)
                return;
        }

        // Place one item into the hovered slot (empty or matching type)
        ghostSlot.MoveTo(hoveredSlot, 1);
        PlaySoundWithCooldown("event:/SFX/Inventory/4_13_SplitStack");
    }

    /// <summary>
    /// Plays a sound effect with a cooldown to prevent audio spam.
    /// </summary>
    private static void PlaySoundWithCooldown(string soundEvent)
    {
        float currentTime = Time.time;
        if (currentTime - lastSoundTime < SOUND_COOLDOWN)
            return;

        try
        {
            GameManager.AudioController.PlaySound(soundEvent);
            lastSoundTime = currentTime;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Failed to play sound: {ex.Message}");
        }
    }
}