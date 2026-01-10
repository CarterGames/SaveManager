using System.Collections.Generic;
using CarterGames.Assets.SaveManager.Backups;
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static partial class SaveManagerSettingsProvider
    {
        private static readonly GUIContent MaxBackups = new GUIContent("Total Save Backups","Defines the number of backups the system makes of the user's save state.");
        private static readonly GUIContent SaveLocationType = new GUIContent("Save Location","Defines the handler used for the save file location.");
        private static readonly GUIContent BackupSaveLocationType = new GUIContent("Backup Save Location","Defines the handler used for the backup save files, this will match the .");

        private static IEnumerable<ISaveBackupLocation> backupLocations;
        
        public static readonly Evt<ISaveBackupLocation, ISaveBackupLocation> BackupSaveLocationChangedEvt = new Evt<ISaveBackupLocation, ISaveBackupLocation>();


        private static void DrawSaveBackupSettings()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Backup Settings", EditorStyles.boldLabel);
            GUILayout.Space(1.5f);
            
            
            EditorGUILayout.PropertyField(SettingsAssetObject.Fp("maxBackups"), MaxBackups);
            
            // Backup location setting
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(BackupSaveLocationType, SettingsAssetObject.Fp("backupLocation").Fpr("type").stringValue);
            EditorGUI.EndDisabledGroup();
            
            if (GUILayout.Button("Select", GUILayout.Width(100)))
            {
                SearchProviderSaveBackupLocations.GetProvider().SelectionMade.Add(OnBackupSaveLocationSelectionMade);
                SearchProviderSaveBackupLocations.GetProvider().Open();
                return;
            }
            
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
        
        
        private static void OnBackupSaveLocationSelectionMade(SearchTreeEntry entry)
        {
            SearchProviderSaveLocations.GetProvider().SelectionMade.Remove(OnSaveLocationSelectionMade);
            
            var oldAssembly = SettingsAssetObject.Fp("backupLocation").Fpr("assembly").stringValue;
            var oldType = SettingsAssetObject.Fp("backupLocation").Fpr("type").stringValue;
            var oldHandler = new AssemblyClassDef(oldAssembly, oldType).GetDefinedType<ISaveBackupLocation>();
            var selectedHandler = (ISaveBackupLocation)entry.userData;

            if (oldHandler == selectedHandler) return;
            
            SettingsAssetObject.Fp("backupLocation").Fpr("assembly").stringValue = selectedHandler.GetType().Assembly.FullName;
            SettingsAssetObject.Fp("backupLocation").Fpr("type").stringValue = selectedHandler.GetType().FullName;

            SettingsAssetObject.ApplyModifiedProperties();
            SettingsAssetObject.Update();
            
            BackupSaveLocationChangedEvt.Raise(oldHandler, selectedHandler);
        }
    }
}