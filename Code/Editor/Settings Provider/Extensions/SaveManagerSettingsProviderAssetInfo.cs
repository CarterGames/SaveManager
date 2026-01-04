using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static partial class SaveManagerSettingsProvider
    {
        private static readonly GUIContent VersionTitle = new GUIContent("Version", "The version of the asset in use.");
        private static readonly GUIContent VersionValue = new GUIContent(AssetVersionData.VersionNumber, "The version you currently have installed.");
        private static readonly GUIContent ReleaseTitle = new GUIContent("Release Date", "The date this version of the asset was published on.");
        private static readonly GUIContent ReleaseValue = new GUIContent(AssetVersionData.ReleaseDate, "The version you currently have installed.");

        
        
        /// <summary>
        /// Draws the info section of the window.
        /// </summary>
        private static void DrawInfo()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Asset Version Info", EditorStyles.boldLabel);
            
            EditorGUI.BeginDisabledGroup(true);
            
            EditorGUILayout.TextField(VersionTitle,  VersionValue.text);
            EditorGUILayout.TextField(ReleaseTitle, ReleaseValue.text);
            
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(17.5f);
            VersionEditorGUI.DrawCheckForUpdatesButton();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
    }
}