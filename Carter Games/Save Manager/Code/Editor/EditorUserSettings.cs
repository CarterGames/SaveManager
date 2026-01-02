/*
 * Save Manager (3.x)
 * Copyright (c) 2025-2026 Carter Games
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version. 
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. 
 *
 * You should have received a copy of the GNU General Public License along with this program.
 * If not, see <https://www.gnu.org/licenses/>. 
 */

using CarterGames.Shared.SaveManager.Editor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class EditorUserSettings
    {
        public static bool GetBool(string key) => (bool) PerUserSettingsEditor.GetOrCreateValue<bool>(key, PerUserSettingType.SessionState);
        public static void SetBool(string key, bool value) => PerUserSettingsEditor.SetValue<bool>(key, PerUserSettingType.SessionState, value);
        
        public static int GetInt(string key) => (int) PerUserSettingsEditor.GetOrCreateValue<int>(key, PerUserSettingType.SessionState);
        public static void SetInt(string key, int value) => PerUserSettingsEditor.SetValue<int>(key, PerUserSettingType.SessionState, value);
        
        public static Vector2 GetVec2(string key) => (Vector2) PerUserSettingsEditor.GetOrCreateValue<Vector2>(key, PerUserSettingType.SessionState, new Vector2());
        public static void SetVec2(string key, Vector2 value) => PerUserSettingsEditor.SetValue<Vector2>(key, PerUserSettingType.SessionState, value);
    }
}