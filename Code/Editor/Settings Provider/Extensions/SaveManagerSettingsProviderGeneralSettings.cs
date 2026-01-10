using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static partial class SaveManagerSettingsProvider
    {
        private static readonly GUIContent DebugLogs = new GUIContent("Show Debug Logs", "Defines if the asset will log any messages to the console. Errors will still go through for visibility.");
        private static readonly GUIContent DevDebugLogs = new GUIContent("Show Dev Logs", "Defines if the asset developer logs show in console. Useful for debugging the asset itself, but not for general use issues.");
        
        
        private static void DrawEditorSettings()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
            GUILayout.Space(1.5f);
            
            SaveManagerPrefs.SetKey(SaveManagerConstants.LogsPref, EditorGUILayout.Toggle(DebugLogs, SaveManagerPrefs.GetBoolKey(SaveManagerConstants.LogsPref)));
            SaveManagerPrefs.SetKey(SaveManagerConstants.DevLogsPref, EditorGUILayout.Toggle(DevDebugLogs, SaveManagerPrefs.GetBoolKey(SaveManagerConstants.DevLogsPref)));
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
    }
}