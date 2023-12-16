using System.IO;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class SaveProfileManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public static void CaptureProfile(string savePath, string captureName)
        {
            var profileSavePath = $"{savePath}/{captureName}.json";
            
            var json = JsonUtility.ToJson(UtilEditor.Settings.SaveData.SerializableData, true);
            
            FileEditorUtil.CreateToDirectory(profileSavePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            File.WriteAllText(profileSavePath, json);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(profileSavePath);

            UtilEditor.SaveProfiles.AddProfile(asset);
            EditorUtility.SetDirty(UtilEditor.SaveProfiles);
        }
    }
}