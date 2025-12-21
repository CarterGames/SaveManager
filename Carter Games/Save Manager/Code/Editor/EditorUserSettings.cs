using CarterGames.Shared.SaveManager.Editor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class EditorUserSettings
    {
        public static bool GetBool(string key) => (bool) PerUserSettingsEditor.GetOrCreateValue<bool>(key, PerUserSettingType.SessionState);
        public static void SetBool(string key, bool value) => PerUserSettingsEditor.SetValue<bool>(key, PerUserSettingType.SessionState, value);
        
        public static int GetInt(string key) => (int) PerUserSettingsEditor.GetOrCreateValue<int>(key, PerUserSettingType.SessionState);
        public static void SetInt(string key, int value) => PerUserSettingsEditor.SetValue<int>(key, PerUserSettingType.SessionState, value);
        
        public static Vector2 GetVec2(string key) => (Vector2) PerUserSettingsEditor.GetOrCreateValue<Vector2>(key, PerUserSettingType.SessionState, new Vector2());
        public static void SetVec2(string key, Vector2 value) => PerUserSettingsEditor.SetValue<Vector2>(key, PerUserSettingType.SessionState, value);
    }
}