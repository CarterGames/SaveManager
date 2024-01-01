/*
 * Copyright (c) 2018-Present Carter Games
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the editor settings per user, instead of an editor asset like before. 
    /// </summary>
    public static class PerUserSettings
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string UniqueIdId = "CarterGames_SaveManager_Editor_UUID";
        
        private static readonly string AutoValidationAutoCheckId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_AutoVersionCheck";
        
        private static readonly string EditorSettingsEditorTabId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_EditorDropdown";
        private static readonly string EditorSettingsSaveOptionsTabId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_SaveOptionsDropdown";
        
        private static readonly string SaveEditorTabPosId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_EditorTabPos";
        private static readonly string SaveEditorProfileCreatorId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_EditorProfileCreatorShown";
        private static readonly string SaveEditorProfileViewerId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_EditorProfileViewerShown";
        private static readonly string SaveLastProfileNameId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_LastProfileName";
        private static readonly string ShowSaveKeysId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_ShowSaveKeys";
        
        private static readonly string LastSaveObjectNameId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_LastSaveObjectName";
        private static readonly string LastSaveObjectFileNameId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_LastSaveObjectFileName";
        private static readonly string JustCreatedSaveObjectId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_JustCreatedSaveObject";
        
        private static readonly string SaveEditorTabScrollRectPosId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_EditorTabScrollRectPos";
        private static readonly string SaveProfileTabScrollRectPosId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_ProfileTabScrollRectPos";
        
        private static readonly string SaveEditorOrganiseByCategoryId = $"{UniqueId}_CarterGames_SaveManager_EditorSettings_OrganiseByCategory";

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The unique if for the assets settings to be per project...
        /// </summary>
        /// <remarks>
        /// Saved to player pref to allow settings to be different per project in the same editor version.
        /// </remarks>
        private static string UniqueId => (string)GetOrCreateValue<string>(UniqueIdId, SettingType.PlayerPref, Guid.NewGuid().ToString());
        
        
        /// <summary>
        /// Has the scanner been initialized.
        /// </summary>
        public static bool VersionValidationAutoCheckOnLoad
        {
            get => (bool) GetOrCreateValue<bool>(AutoValidationAutoCheckId, SettingType.EditorPref, true);
            set => SetValue<bool>(AutoValidationAutoCheckId, SettingType.EditorPref, value);
        }
        
        
        /// <summary>
        /// If the editor tab in the editor window is active or not.
        /// </summary>
        public static bool EditorSettingsEditorTab
        {
            get => (bool)GetOrCreateValue<bool>(EditorSettingsEditorTabId, SettingType.EditorPref);
            set => SetValue<bool>(EditorSettingsEditorTabId, SettingType.EditorPref, value);
        }
        
        
        /// <summary>
        /// If the save options tab in the editor window is active or not.
        /// </summary>
        public static bool EditorSettingsSaveOptionsTab
        {
            get => (bool)GetOrCreateValue<bool>(EditorSettingsSaveOptionsTabId, SettingType.EditorPref);
            set => SetValue<bool>(EditorSettingsSaveOptionsTabId, SettingType.EditorPref, value);
        }
        
        
        /// <summary>
        /// The tab currently open in the save editor.
        /// </summary>
        public static int SaveEditorTabPos
        {
            get => (int)GetOrCreateValue<int>(SaveEditorTabPosId, SettingType.EditorPref);
            set => SetValue<int>(SaveEditorTabPosId, SettingType.EditorPref, value);
        }
        
        
        /// <summary>
        /// If the save profile creator dropdown is open.
        /// </summary>
        public static bool SaveEditorProfileCreator
        {
            get => (bool)GetOrCreateValue<bool>(SaveEditorProfileCreatorId, SettingType.EditorPref);
            set => SetValue<bool>(SaveEditorProfileCreatorId, SettingType.EditorPref, value);
        }
        
        
        /// <summary>
        /// If the save viewer dropdown is open.
        /// </summary>
        public static bool SaveEditorProfileViewer
        {
            get => (bool)GetOrCreateValue<bool>(SaveEditorProfileViewerId, SettingType.EditorPref);
            set => SetValue<bool>(SaveEditorProfileViewerId, SettingType.EditorPref, value);
        }
        
        
        /// <summary>
        /// The last profile name used in the profile creator.
        /// </summary>
        public static string LastProfileName
        {
            get => (string)GetOrCreateValue<string>(SaveLastProfileNameId, SettingType.EditorPref);
            set => SetValue<string>(SaveLastProfileNameId, SettingType.EditorPref, value);
        }
        
        
        /// <summary>
        /// Should the editor show the save keys.
        /// </summary>
        public static bool ShowSaveKeys
        {
            get => (bool)GetOrCreateValue<bool>(ShowSaveKeysId, SettingType.EditorPref);
            set => SetValue<bool>(ShowSaveKeysId, SettingType.EditorPref, value);
        }
        
        
        /// <summary>
        /// The name of last save object made by the user.
        /// </summary>
        public static string LastSaveObjectName
        {
            get => (string)GetOrCreateValue<string>(LastSaveObjectNameId, SettingType.EditorPref);
            set => SetValue<string>(LastSaveObjectNameId, SettingType.EditorPref, value);
        }
        
        
        /// <summary>
        /// The file name of the last save object made by the user.
        /// </summary>
        public static string LastSaveObjectFileName
        {
            get => (string)GetOrCreateValue<string>(LastSaveObjectFileNameId, SettingType.EditorPref);
            set => SetValue<string>(LastSaveObjectFileNameId, SettingType.EditorPref, value);
        }
        
        
        /// <summary>
        /// Defines if the user has just made a new save object with the menu item or not.
        /// </summary>
        public static bool JustCreatedSaveObject
        {
            get => (bool)GetOrCreateValue<bool>(JustCreatedSaveObjectId, SettingType.EditorPref);
            set => SetValue<bool>(JustCreatedSaveObjectId, SettingType.EditorPref, value);
        }
        
        
        public static Vector2 SaveEditorTabScrollRectPos
        {
            get => (Vector2)GetOrCreateValue<Vector2>(SaveEditorTabScrollRectPosId, SettingType.EditorPref);
            set => SetValue<Vector2>(SaveEditorTabScrollRectPosId, SettingType.EditorPref, value);
        }
        
        
        public static Vector2 SaveProfileTabScrollRectPos
        {
            get => (Vector2)GetOrCreateValue<Vector2>(SaveProfileTabScrollRectPosId, SettingType.EditorPref);
            set => SetValue<Vector2>(SaveProfileTabScrollRectPosId, SettingType.EditorPref, value);
        }
        
        
        public static bool SaveEditorOrganiseByCategory
        {
            get => (bool)GetOrCreateValue<bool>(SaveEditorOrganiseByCategoryId, SettingType.EditorPref, true);
            set => SetValue<bool>(SaveEditorOrganiseByCategoryId, SettingType.EditorPref, value);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static object GetOrCreateValue<T>(string key, SettingType type, object defaultValue = null)
        {
            switch (type)
            {
                case SettingType.EditorPref:

                    if (EditorPrefs.HasKey(key))
                    {
                        switch (typeof(T))
                        {
                            case var x when x == typeof(bool):
                                return EditorPrefs.GetBool(key);
                            case var x when x == typeof(int):
                                return EditorPrefs.GetInt(key);
                            case var x when x == typeof(float):
                                return EditorPrefs.GetFloat(key);
                            case var x when x == typeof(string):
                                return EditorPrefs.GetString(key);
                            case var x when x == typeof(Vector2):
                                return JsonUtility.FromJson<Vector2>(EditorPrefs.GetString(key));
                            default:
                                return null;
                        }
                    }

                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            EditorPrefs.SetBool(key, defaultValue == null ? false : (bool)defaultValue);
                            return EditorPrefs.GetBool(key);
                        case var x when x == typeof(int):
                            EditorPrefs.SetInt(key, defaultValue == null ? 0 : (int)defaultValue);
                            return EditorPrefs.GetInt(key);
                        case var x when x == typeof(float):
                            EditorPrefs.SetFloat(key, defaultValue == null ? 0 : (float)defaultValue);
                            return EditorPrefs.GetFloat(key);
                        case var x when x == typeof(string):
                            EditorPrefs.SetString(key, (string)defaultValue);
                            return EditorPrefs.GetString(key);
                        case var x when x == typeof(Vector2):
                            EditorPrefs.SetString(key,
                                defaultValue == null
                                    ? JsonUtility.ToJson(Vector2.zero)
                                    : JsonUtility.ToJson(defaultValue));
                            return JsonUtility.FromJson<Vector2>(EditorPrefs.GetString(key));
                        default:
                            return null;
                    }
                    
                case SettingType.PlayerPref:
                    
                    if (PlayerPrefs.HasKey(key))
                    {
                        switch (typeof(T))
                        {
                            case var x when x == typeof(bool):
                                return PlayerPrefs.GetInt(key) == 1;
                            case var x when x == typeof(int):
                                return PlayerPrefs.GetInt(key);
                            case var x when x == typeof(float):
                                return PlayerPrefs.GetFloat(key);
                            case var x when x == typeof(string):
                                return PlayerPrefs.GetString(key);
                            case var x when x == typeof(Vector2):
                                return JsonUtility.FromJson<Vector2>(PlayerPrefs.GetString(key));
                            default:
                                return null;
                        }
                    }

                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            PlayerPrefs.SetInt(key,
                                defaultValue == null ? 0 : defaultValue.ToString().ToLower() == "true" ? 1 : 0);
                            return PlayerPrefs.GetInt(key) == 1;
                        case var x when x == typeof(int):
                            PlayerPrefs.SetInt(key, defaultValue == null ? 0 : (int)defaultValue);
                            return PlayerPrefs.GetInt(key);
                        case var x when x == typeof(float):
                            PlayerPrefs.SetFloat(key, defaultValue == null ? 0 : (float)defaultValue);
                            return PlayerPrefs.GetFloat(key);
                        case var x when x == typeof(string):
                            PlayerPrefs.SetString(key, (string)defaultValue);
                            return PlayerPrefs.GetString(key);
                        case var x when x == typeof(Vector2):
                            PlayerPrefs.SetString(key,
                                defaultValue == null
                                    ? JsonUtility.ToJson(Vector2.zero)
                                    : JsonUtility.ToJson(defaultValue));
                            return JsonUtility.FromJson<Vector2>(PlayerPrefs.GetString(key));
                        default:
                            return null;
                    }
                    
                case SettingType.SessionState:

                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            return SessionState.GetBool(key, defaultValue == null ? false : (bool)defaultValue);
                        case var x when x == typeof(int):
                            return SessionState.GetInt(key, defaultValue == null ? 0 : (int)defaultValue);
                        case var x when x == typeof(float):
                            return SessionState.GetFloat(key, defaultValue == null ? 0 : (float)defaultValue);
                        case var x when x == typeof(string):
                            return SessionState.GetString(key, (string)defaultValue);
                        case var x when x == typeof(Vector2):
                            return JsonUtility.FromJson<Vector2>(SessionState.GetString(key,
                                JsonUtility.ToJson(defaultValue)));
                        default:
                            return null;
                    }
                    
                default:
                    return null;
            }
        }


        private static void SetValue<T>(string key, SettingType type, object value)
        {
            switch (type)
            {
                case SettingType.EditorPref:
                    
                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            EditorPrefs.SetBool(key, (bool)value);
                            break;
                        case var x when x == typeof(int):
                            EditorPrefs.SetInt(key, (int)value);
                            break;
                        case var x when x == typeof(float):
                            EditorPrefs.SetFloat(key, (float)value);
                            break;
                        case var x when x == typeof(string):
                            EditorPrefs.SetString(key, (string)value);
                            break;
                        case var x when x == typeof(Vector2):
                            EditorPrefs.SetString(key, JsonUtility.ToJson(value));
                            break;
                    }
                    
                    break;
                case SettingType.PlayerPref:
                    
                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            PlayerPrefs.SetInt(key, ((bool)value) ? 1 : 0);
                            break;
                        case var x when x == typeof(int):
                            PlayerPrefs.SetInt(key, (int)value);
                            break;
                        case var x when x == typeof(float):
                            PlayerPrefs.SetFloat(key, (float)value);
                            break;
                        case var x when x == typeof(string):
                            PlayerPrefs.SetString(key, (string)value);
                            break;
                        case var x when x == typeof(Vector2):
                            PlayerPrefs.SetString(key, JsonUtility.ToJson(value));
                            break;
                    }
                    
                    PlayerPrefs.Save();
                    
                    break;
                case SettingType.SessionState:
                    
                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            SessionState.SetBool(key, (bool)value);
                            break;
                        case var x when x == typeof(int):
                            SessionState.SetInt(key, (int)value);
                            break;
                        case var x when x == typeof(float):
                            SessionState.SetFloat(key, (float)value);
                            break;
                        case var x when x == typeof(string):
                            SessionState.SetString(key, (string)value);
                            break;
                        case var x when x == typeof(Vector2):
                            SessionState.SetString(key, JsonUtility.ToJson(value));
                            break;
                    }
                    
                    break;
            }
        }


        /// <summary>
        /// Resets all user editor settings for this asset when called.
        /// </summary>
        [MenuItem("Tools/Carter Games/Save Manager/Reset User Editor Settings")]
        public static void ResetPrefs()
        {
            SaveEditorTabPos = 0;
            SaveEditorProfileCreator = false;
            SaveEditorProfileViewer = false;
            LastProfileName = string.Empty;
            ShowSaveKeys = false;
            LastSaveObjectName = string.Empty;
            LastSaveObjectFileName = string.Empty;
            JustCreatedSaveObject = false;
            SaveEditorOrganiseByCategory = true;
        }
    }
}