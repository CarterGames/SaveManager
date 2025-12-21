using System.Collections.Generic;
using System.Linq;
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
        
        
        public static readonly Evt<ISaveDataLocation, ISaveDataLocation> SaveLocationChangedEvt = new Evt<ISaveDataLocation, ISaveDataLocation>();


        private static void DrawSaveBackupSettings()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Backup Settings", EditorStyles.boldLabel);
            GUILayout.Space(1.5f);
            
            
            EditorGUILayout.PropertyField(SettingsAssetObject.Fp("maxBackups"), MaxBackups);
            
            // Backup location setting
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(BackupSaveLocationType, SettingsAssetObject.Fp("backupLocation").Fpr("type").stringValue);
            EditorGUI.EndDisabledGroup();
            
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
    }
}