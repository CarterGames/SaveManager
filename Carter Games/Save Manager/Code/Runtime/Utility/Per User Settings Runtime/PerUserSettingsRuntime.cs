/*
 * Copyright (c) 2024 Carter Games
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

using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles the per user settings that are for runtime use.
    /// </summary>
    public static class PerUserSettingsRuntime
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static readonly string DebugLogId = "CarterGames_SaveManager_Settings_ShowDebugLogs";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Should the asset show the debug logs.
        /// </summary>
        public static bool ShowDebugLogs
        {
            get => (bool) GetOrCreateValue<bool>(DebugLogId, true);
            set => SetValue<bool>(DebugLogId, value);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private static object GetOrCreateValue<T>(string key, object defaultValue = null)
        {
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
                    PlayerPrefs.SetInt(key, defaultValue == null ? 0 : defaultValue.ToString().ToLower() == "true" ? 1 : 0);                    
                    return PlayerPrefs.GetInt(key) == 1;
                case var x when x == typeof(int):
                    PlayerPrefs.SetInt(key, defaultValue == null ? 0 : (int) defaultValue);
                    return PlayerPrefs.GetInt(key);
                case var x when x == typeof(float):
                    PlayerPrefs.SetFloat(key, defaultValue == null ? 0 : (float) defaultValue);
                    return PlayerPrefs.GetFloat(key);
                case var x when x == typeof(string):
                    PlayerPrefs.SetString(key, (string) defaultValue);
                    return PlayerPrefs.GetString(key);
                case var x when x == typeof(Vector2):
                    PlayerPrefs.SetString(key, defaultValue == null ? JsonUtility.ToJson(Vector2.zero) : JsonUtility.ToJson(defaultValue));
                    return JsonUtility.FromJson<Vector2>(PlayerPrefs.GetString(key));
                default:
                    return null;
            }
        }


        private static void SetValue<T>(string key, object value)
        {
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
                default:
                    break;
            }
            
            PlayerPrefs.Save();
        }


        public static void ResetPrefs()
        {
            ShowDebugLogs = true;
        }
    }
}