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

using UnityEditor;
using UnityEngine;

namespace CarterGames.Shared.SaveManager.Editor
{
    public static class PerUserSettingsEditor
    {
        public static object GetOrCreateValue<T>(string key, PerUserSettingType type, object defaultValue = null)
        {
            switch (type)
            {
                case PerUserSettingType.EditorPref:

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
                    
                case PerUserSettingType.PlayerPref:
                    
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
                    
                case PerUserSettingType.SessionState:

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


        public static void SetValue<T>(string key, PerUserSettingType type, object value)
        {
            switch (type)
            {
                case PerUserSettingType.EditorPref:
                    
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
                case PerUserSettingType.PlayerPref:
                    
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
                case PerUserSettingType.SessionState:
                    
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
    }
}