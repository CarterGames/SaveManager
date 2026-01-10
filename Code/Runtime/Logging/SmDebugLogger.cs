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

using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A logging class for messages within the asset.
    /// </summary>
    public static class SmDebugLogger
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constants
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        private const string LogPrefix = "<color=#4479D9><b>Save Manager</b></color> | ";
        private const string WarningPrefix = "<color=#D6BA64><b>Warning</b></color> | ";
        private const string ErrorPrefix = "<color=#E77A7A><b>Error</b></color> | ";
        private const string DevPrefix = "<color=#8a6cba><b>Dev</b></color> | ";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if the logs should be shown.
        /// </summary>
        private static bool ShowLogs => SaveManagerPrefs.GetBoolKey(SaveManagerConstants.LogsPref);
        
        
        /// <summary>
        /// Gets if the dev logs should be shown.
        /// </summary>
        private static bool ShowDevLogs => SaveManagerPrefs.GetBoolKey(SaveManagerConstants.DevLogsPref);
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        /// <summary>
        /// Displays a normal debug message for the asset.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public static void Log(string message)
        {
            if (!ShowLogs) return;
            Debug.Log($"{LogPrefix}{message}");
        }
        
        
        /// <summary>
        /// Displays a warning debug message for the asset.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public static void LogWarning(string message) 
        {
            if (!ShowLogs) return;
            Debug.LogWarning($"{LogPrefix}{WarningPrefix}{message}");
        }
        
        
        /// <summary>
        /// Displays a error debug message for the asset.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public static void LogError(string message)
        {
            Debug.LogError($"{LogPrefix}{ErrorPrefix}{message}");
        }


        /// <summary>
        /// Displays an internal log intended for dev use only.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public static void LogDev(string message)
        {
            if (!ShowDevLogs) return;
            Debug.Log($"{LogPrefix}{DevPrefix}{message}"); 
        }
    }
}