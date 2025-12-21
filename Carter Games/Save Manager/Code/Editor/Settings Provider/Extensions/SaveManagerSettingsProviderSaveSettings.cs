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
        private static readonly GUIContent AutoSave = new GUIContent("Auto Save On Exit?","Defines if the game tries to save when exiting the game.");
        private static readonly GUIContent EditorSaveFile = new GUIContent("Editor Save File","The path to the editor save file.");

        
        private static void DrawSaveSettings()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Save Settings", EditorStyles.boldLabel);
            GUILayout.Space(1.5f);

            
            // General.
            EditorGUILayout.PropertyField(SettingsAssetObject.Fp("autoSaveOnExit"), AutoSave);
            EditorGUILayout.PropertyField(SettingsAssetObject.Fp("useJsonConverters"));
            
            
            // Save Location handler.
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(SaveLocationType, SettingsAssetObject.Fp("saveLocation").Fpr("type").stringValue);

            EditorGUI.EndDisabledGroup();
            
            if (GUILayout.Button("Select", GUILayout.Width(100)))
            {
                SearchProviderSaveLocations.GetProvider().SelectionMade.Add(OnSaveLocationSelectionMade);
                SearchProviderSaveLocations.GetProvider().Open();
                return;
            }
            
            EditorGUILayout.EndHorizontal();
            
            
            // Editor Save.
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.TextField(EditorSaveFile, ScriptableRef.GetAssetDef<DataAssetSettings>().DataAssetRef.SavePath);
            
            if (GUILayout.Button("Open Path",GUILayout.Width(100)))
            {
                Application.OpenURL(ScriptableRef.GetAssetDef<DataAssetSettings>().DataAssetRef.SavePath.Replace("save.sf2", string.Empty));
            }
            
            if (GUILayout.Button("Open File",GUILayout.Width(100)))
            {
                Application.OpenURL(ScriptableRef.GetAssetDef<DataAssetSettings>().DataAssetRef.SavePath);
            }
            
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
        
        
        private static void OnSaveLocationSelectionMade(SearchTreeEntry entry)
        {
            SearchProviderSaveLocations.GetProvider().SelectionMade.Remove(OnSaveLocationSelectionMade);
            
            var selectedHandler = (ISaveDataLocation)entry.userData;

            if (backupLocations == null)
            {
                backupLocations = AssemblyHelper.GetClassesOfType<ISaveBackupLocation>(false);
            }

            if (selectedHandler.GetType().FullName == SettingsAssetObject.Fp("saveLocation").Fpr("type").stringValue)
            {
                return;
            }

            var oldAssembly = SettingsAssetObject.Fp("saveLocation").Fpr("assembly").stringValue;
            var oldType = SettingsAssetObject.Fp("saveLocation").Fpr("type").stringValue;
            var oldHandler = new AssemblyClassDef(oldAssembly, oldType).GetDefinedType<ISaveDataLocation>();

            var backupHandler = backupLocations.FirstOrDefault(t => t.Location.GetType() == selectedHandler.DataLocation.GetType());

            SettingsAssetObject.Fp("saveLocation").Fpr("assembly").stringValue = selectedHandler.GetType().Assembly.FullName;
            SettingsAssetObject.Fp("saveLocation").Fpr("type").stringValue = selectedHandler.GetType().FullName;

            if (backupHandler != null)
            {
                SettingsAssetObject.Fp("backupLocation").Fpr("assembly").stringValue = backupHandler.GetType().Assembly.FullName;
                SettingsAssetObject.Fp("backupLocation").Fpr("type").stringValue = backupHandler.GetType().FullName;
            }

            SettingsAssetObject.ApplyModifiedProperties();
            SettingsAssetObject.Update();
            
            SaveLocationChangedEvt.Raise(oldHandler, selectedHandler);
        }
    }
}