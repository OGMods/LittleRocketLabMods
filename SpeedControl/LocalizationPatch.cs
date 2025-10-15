using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace SpeedControl;

[HarmonyPatch(typeof(LocalizationController), "Awake")]
public class LocalizationController_Awake_Patch
{
    private static bool csvAdded = false;

    [HarmonyPostfix]
    public static void Postfix(LocalizationController __instance)
    {
        if (!csvAdded && __instance != null && __instance._textRepository != null)
        {
            try
            {
                MelonLogger.Msg("Adding custom localization CSV for Speed Control...");

                // Create CSV content for Speed Control notification
                string csvContent = "Table,Key,Description,Translate?,English,French,Italian,German,Spanish,Simplified Chinese,Thai\n" +
                                  "NotificationController,SpeedChanged,Speed Control change notification,Yes,Speed Control <color=#0061a6>({0})</color>,Échelle de temps <color=#0061a6>({0})</color>,Scala temporale <color=#0061a6>({0})</color>,Zeitskala <color=#0061a6>({0})</color>,Escala de tiempo <color=#0061a6>({0})</color>,时间比例 <color=#0061a6>({0})</color>,มาตราส่วนเวลา <color=#0061a6>({0})</color>";

                // Create a TextAsset with our CSV content
                var textAsset = new TextAsset(csvContent);
                textAsset.name = "SpeedControl_Localization";

                // Add the CSV file to the text repository
                __instance._textRepository.AddCsvFile(textAsset);

                csvAdded = true;
                MelonLogger.Msg("Successfully added custom localization CSV for SpeedChanged");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"Failed to add custom localization CSV: {ex.Message}");
                MelonLogger.Error($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
