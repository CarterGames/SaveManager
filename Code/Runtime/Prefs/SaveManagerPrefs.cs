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

using System.Globalization;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A player pref helper class for the Save Manager specifically.
    /// </summary>
    public static class SaveManagerPrefs
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        /// <summary>
        /// Gets a int keys value.
        /// </summary>
        /// <param name="key">The key to get for.</param>
        /// <returns>Int</returns>
        public static int GetIntKey(string key) => int.Parse(Internal_GetKey<int>(key));
        
        
        /// <summary>
        /// Gets a bool keys value.
        /// </summary>
        /// <param name="key">The key to get for.</param>
        /// <returns>Bool</returns>
        public static bool GetBoolKey(string key) => bool.Parse(Internal_GetKey<bool>(key));
        
        
        /// <summary>
        /// Gets a float keys value.
        /// </summary>
        /// <param name="key">The key to get for.</param>
        /// <returns>Float</returns>
        public static float GetFloatKey(string key) => float.Parse(Internal_GetKey<float>(key));
        
        
        /// <summary>
        /// Gets a string keys value.
        /// </summary>
        /// <param name="key">The key to get for.</param>
        /// <returns>String</returns>
        public static string GetStringKey(string key) => Internal_GetKey<string>(key);
        
        
        /// <summary>
        /// Sets a int key to the entered value.
        /// </summary>
        /// <param name="key">The key to set</param>
        /// <param name="value">The value to set the entered key to.</param>
        public static void SetKey(string key, int value) => Internal_SetKey<int>(key, value.ToString());
        
        
        /// <summary>
        /// Sets a bool key to the entered value.
        /// </summary>
        /// <param name="key">The key to set</param>
        /// <param name="value">The value to set the entered key to.</param>
        public static void SetKey(string key, bool value) => Internal_SetKey<bool>(key, value.ToString());
        
        
        /// <summary>
        /// Sets a float key to the entered value.
        /// </summary>
        /// <param name="key">The key to set</param>
        /// <param name="value">The value to set the entered key to.</param>
        public static void SetKey(string key, float value) => Internal_SetKey<float>(key, value.ToString(CultureInfo.InvariantCulture));
        
        
        /// <summary>
        /// Sets a string key to the entered value.
        /// </summary>
        /// <param name="key">The key to set</param>
        /// <param name="value">The value to set the entered key to.</param>
        public static void SetKey(string key, string value) => Internal_SetKey<string>(key, value);
        
        
        /// <summary>
        /// Handles getting the key in the proper format for use.
        /// </summary>
        /// <param name="key">The key to get fully.</param>
        /// <returns>The complete key.</returns>
        private static string Internal_GetKey(string key) => $"{SaveManagerConstants.PrefFormat}_{key}";
        
        
        /// <summary>
        /// Gets a keys value.
        /// </summary>
        /// <param name="key">The key to get.</param>
        /// <typeparam name="T">The type the key is.</typeparam>
        /// <returns>The key's value in JSON</returns>
        private static string Internal_GetKey<T>(string key)
        {
            var prefKey = Internal_GetKey(key);
            
            if (PlayerPrefs.HasKey(prefKey))
            {
                return PlayerPrefs.GetString(prefKey);
            }

            PlayerPrefs.SetString(prefKey, default(T)?.ToString());
            return default(T)?.ToString();
        }
        
        
        /// <summary>
        /// Set a key to a value.
        /// </summary>
        /// <param name="key">The key to set.</param>
        /// <param name="value">The value to set the key to.</param>
        /// <typeparam name="T">The type to set.</typeparam>
        private static void Internal_SetKey<T>(string key, string value)
        {
            PlayerPrefs.SetString(Internal_GetKey(key), value);
        }
    }
}