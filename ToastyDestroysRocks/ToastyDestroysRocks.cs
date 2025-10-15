using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(ToastyDestroysRocks.ToastyDestroysRocksMod), "Toasty Destroys Rocks", "1.0.0", "OGMods")]
[assembly: MelonGame(null, null)]

namespace ToastyDestroysRocks;

public class ToastyDestroysRocksMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("Toasty Destroys Rocks loaded!");
    }
}

[HarmonyPatch(typeof(MechController), nameof(MechController.AttemptToRemoveBuiding))]
public class MechController_AttemptToRemoveBuilding_Patch
{
    // List of protected rock types that cannot be removed by Toasty
    private static readonly string[] ProtectedRockTypes = new string[]
    {
        "RockStone2Building",
        "RockStone3Building",
        "RockCopper2Building",
        "RockCopper3Building",
        "RockIron2Building",
        "RockIron3Building",
        "RockCoal2Building",
        "RockCoal3Building",
    };

    static bool Prefix(MechController __instance, Vector3Int tilePosition, GridLayer layer, ref bool __result)
    {
        try
        {
            var factory = GameManager.CurrentFactory;
            if (factory == null) return false;
            
            // Check if building is allowed
            if (!factory.AllowsBuilding)
            {
                __result = false;
                return false;
            }

            // Get building at position and if building can be removed
            var building = factory.GetBuildingAtPosition(tilePosition, layer);
            if (building == null || !building.CanBeRemoved || building.RemovedUsingActionType != ActionType.Hammer)
            {
                __result = false;
                return false;
            }

            // Check if this type is in the protected list
            bool isProtected = false;
            foreach (var protectedKey in ProtectedRockTypes)
            {
                if (building.Key == protectedKey)
                {
                    isProtected = true;
                    break;
                }
            }

            if (isProtected)
            {
                var tilemapController = GameManager.BuildingSpriteTilemapController;
                if (tilemapController != null)
                {
                    tilemapController.PlayUnremovableAnimation(building, true);

                    GameManager.AudioController?.PlaySound("event:/SFX/Mech/21_14_InvalidBuilding");
                }

                __result = false;
                return false;
            }

            // Send quest event
            var questCoordinator = GameManager.QuestCoordinator;
            if (questCoordinator != null)
            {
                var removalEventType = building.RemovalEventType;
                questCoordinator.SendEvent(removalEventType, building.Key, null, 1);

                // Remove the building
                factory.RemoveBuilding(tilePosition, layer, true);

                GameManager.AudioController?.PlaySound("event:/SFX/Mech/21_13_DestroyBuilding");

                // Fire visual effects
                var visualEffects = GameManager.VisualEffects;
                if (visualEffects != null)
                {
                    visualEffects.FireRemoveBuildingDustEffect(
                        building.TilemapRootPosition,
                        building.SizeX,
                        building.SizeY
                    );

                    Vector3 effectPosition = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z);
                    visualEffects.FireRemovalEffect(effectPosition, building.RemovalParticleType);
                }

                __result = true;
                return false; // Skip original
            }

            __result = false;
            return false;
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error($"[ToastyDestroysRocks] Error in AttemptToRemoveBuilding patch: {ex}");
            MelonLogger.Error($"[ToastyDestroysRocks] Stack trace: {ex.StackTrace}");
            return true; // Run original on error
        }
    }
}
