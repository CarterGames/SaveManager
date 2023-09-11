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
        
        private const string SaveEditorTabPosId = "CarterGames_SaveManager_EditorSettings_EditorTabPos";
        private const string SaveEditorProfileCreatorId = "CarterGames_SaveManager_EditorSettings_EditorProfileCreatorShown";
        private const string SaveEditorProfileViewerId = "CarterGames_SaveManager_EditorSettings_EditorProfileViewerShown";
        private const string SaveLastProfileNameId = "CarterGames_SaveManager_EditorSettings_LastProfileName";
        private const string ShowSaveKeysId = "CarterGames_SaveManager_EditorSettings_ShowSaveKeys";
        
        private const string LastSaveObjectNameId = "CarterGames_SaveManager_EditorSettings_LastSaveObjectName";
        private const string LastSaveObjectFileNameId = "CarterGames_SaveManager_EditorSettings_LastSaveObjectFileName";
        private const string JustCreatedSaveObjectId = "CarterGames_SaveManager_EditorSettings_JustCreatedSaveObject";

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The tab currently open in the save editor.
        /// </summary>
        public static int SaveEditorTabPos
        {
            get => (int)GetOrCreateValue<int>(SaveEditorTabPosId);
            set => SetValue<int>(SaveEditorTabPosId, value);
        }
        
        
        /// <summary>
        /// If the save profile creator dropdown is open.
        /// </summary>
        public static bool SaveEditorProfileCreator
        {
            get => (bool)GetOrCreateValue<bool>(SaveEditorProfileCreatorId);
            set => SetValue<bool>(SaveEditorProfileCreatorId, value);
        }
        
        
        /// <summary>
        /// If the save viewer dropdown is open.
        /// </summary>
        public static bool SaveEditorProfileViewer
        {
            get => (bool)GetOrCreateValue<bool>(SaveEditorProfileViewerId);
            set => SetValue<bool>(SaveEditorProfileViewerId, value);
        }
        
        
        /// <summary>
        /// The last profile name used in the profile creator.
        /// </summary>
        public static string LastProfileName
        {
            get => (string)GetOrCreateValue<string>(SaveLastProfileNameId);
            set => SetValue<string>(SaveLastProfileNameId, value);
        }
        
        
        /// <summary>
        /// Should the editor show the save keys.
        /// </summary>
        public static bool ShowSaveKeys
        {
            get => (bool)GetOrCreateValue<bool>(ShowSaveKeysId);
            set => SetValue<bool>(ShowSaveKeysId, value);
        }
        
        
        /// <summary>
        /// The name of last save object made by the user.
        /// </summary>
        public static string LastSaveObjectName
        {
            get => (string)GetOrCreateValue<string>(LastSaveObjectNameId);
            set => SetValue<string>(LastSaveObjectNameId, value);
        }
        
        
        /// <summary>
        /// The file name of the last save object made by the user.
        /// </summary>
        public static string LastSaveObjectFileName
        {
            get => (string)GetOrCreateValue<string>(LastSaveObjectFileNameId);
            set => SetValue<string>(LastSaveObjectFileNameId, value);
        }
        
        
        /// <summary>
        /// Defines if the user has just made a new save object with the menu item or not.
        /// </summary>
        public static bool JustCreatedSaveObject
        {
            get => (bool)GetOrCreateValue<bool>(JustCreatedSaveObjectId);
            set => SetValue<bool>(JustCreatedSaveObjectId, value);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// A helper class to get or create a editor pref value.
        /// </summary>
        /// <param name="key">The key to use.</param>
        /// <param name="defaultValue">The default value for the value.</param>
        /// <typeparam name="T">The type to create/get.</typeparam>
        /// <returns>A generic object value of the pref.</returns>
        private static object GetOrCreateValue<T>(string key, object defaultValue = null)
        {
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
                    EditorPrefs.SetBool(key, defaultValue == null ? false : (bool) defaultValue);
                    return EditorPrefs.GetBool(key);
                case var x when x == typeof(int):
                    EditorPrefs.SetInt(key, defaultValue == null ? 0 : (int) defaultValue);
                    return EditorPrefs.GetInt(key);
                case var x when x == typeof(float):
                    EditorPrefs.SetFloat(key, defaultValue == null ? 0 : (float) defaultValue);
                    return EditorPrefs.GetFloat(key);
                case var x when x == typeof(string):
                    EditorPrefs.SetString(key, (string) defaultValue);
                    return EditorPrefs.GetString(key);
                case var x when x == typeof(Vector2):
                    EditorPrefs.SetString(key, defaultValue == null ? JsonUtility.ToJson(Vector2.zero) : JsonUtility.ToJson(defaultValue));
                    return JsonUtility.FromJson<Vector2>(EditorPrefs.GetString(key));
                default:
                    return null;
            }
        }


        /// <summary>
        /// Sets an editor pref to the entered value.
        /// </summary>
        /// <param name="key">The key to use.</param>
        /// <param name="value">The value to set to.</param>
        /// <typeparam name="T">The type to set.</typeparam>
        private static void SetValue<T>(string key, object value)
        {
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
                default:
                    break;
            }
        }


        /// <summary>
        /// Resets all user editor settings for this asset when called.
        /// </summary>
        [MenuItem("Tools/Carter Games/Save Manager/Reset User Editor Settings")]
        private static void ResetPrefs()
        {
            SaveEditorTabPos = 0;
            SaveEditorProfileCreator = false;
            SaveEditorProfileViewer = false;
            LastProfileName = string.Empty;
            ShowSaveKeys = false;
            LastSaveObjectName = string.Empty;
            LastSaveObjectFileName = string.Empty;
            JustCreatedSaveObject = false;
        }
    }
}