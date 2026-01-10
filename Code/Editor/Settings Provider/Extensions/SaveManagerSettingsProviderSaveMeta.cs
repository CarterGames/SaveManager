using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static partial class SaveManagerSettingsProvider
    {
        private static readonly GUIContent MetaGameInfo = new GUIContent("Game Info","Store basic game info (Save Date/Version Number) in the game save?");
        private static readonly GUIContent MetaSystemInfo = new GUIContent("System Info","Store basic system info (OS/CPU/RAM/GPU) in the game save?");


        private static void DrawSaveMetaSettings()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(1.5f);

            EditorGUILayout.LabelField("Save Meta Data", EditorStyles.boldLabel);
            GUILayout.Space(1.5f);

            EditorGUILayout.HelpBox(
                "Implement the ISaveMetaData interface to add your own meta data information to the save file. If none are enabled the metadata section will be omitted from the save. Meta data is intended to be read-only and should be used as such.",
                MessageType.Info);

            // Save meta data.
            EditorGUILayout.PropertyField(
                ScriptableRef.GetAssetDef<DataAssetSettings>().ObjectRef.Fp("useMetaDataGameInfo"), MetaGameInfo);
            EditorGUILayout.PropertyField(
                ScriptableRef.GetAssetDef<DataAssetSettings>().ObjectRef.Fp("useMetaDataSystemInfo"), MetaSystemInfo);

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
    }
}