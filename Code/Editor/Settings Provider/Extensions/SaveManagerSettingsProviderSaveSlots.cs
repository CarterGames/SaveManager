using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static partial class SaveManagerSettingsProvider
    {
        private static readonly GUIContent SaveSlots = new GUIContent("Save Slots","Control save slots and their usage.");


        private static bool UseSaveSlots
        {
            get => SettingsAssetObject.Fp("useSaveSlots").boolValue;
            set => SettingsAssetObject.Fp("useSaveSlots").boolValue = value;
        }
        
        
        private static void DrawSaveSlotSettings()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Save Slots", EditorStyles.boldLabel);
            GUILayout.Space(1.5f);
            
            EditorGUILayout.PropertyField(SettingsAssetObject.Fp("useSaveSlots"));
            
            EditorGUI.BeginDisabledGroup(!UseSaveSlots);
            
            EditorGUILayout.PropertyField(SettingsAssetObject.Fp("limitAvailableSlots"));
            
            EditorGUI.BeginDisabledGroup(!SettingsAssetObject.Fp("limitAvailableSlots").boolValue);
            EditorGUILayout.PropertyField(SettingsAssetObject.Fp("maxUserSaveSlots"));
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.EndDisabledGroup();
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
    }
}