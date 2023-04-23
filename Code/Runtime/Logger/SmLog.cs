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

using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A logging class for messages within the asset.
    /// </summary>
    public static class SmLog
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constants
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        private const string LogPrefix = "<color=#4479D9><b>Save Manager</b></color> | ";
        private const string WarningPrefix = "<color=#D6BA64><b>Warning</b></color> | ";
        private const string ErrorPrefix = "<color=#E77A7A><b>Error</b></color> | ";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if the logs should be shown.
        /// </summary>
        private static bool ShowLogs => AssetAccessor.GetAsset<SettingsAssetRuntime>().ShowLogs;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        /// <summary>
        /// Displays a normal debug message for the build versions asset...
        /// </summary>
        /// <param name="message">The message to show...</param>
        public static void Normal(string message)
        {
            if (!ShowLogs) return;
            Debug.Log($"{LogPrefix}{message}");
        }
        
        
        /// <summary>
        /// Displays a warning debug message for the build versions asset...
        /// </summary>
        /// <param name="message">The message to show...</param>
        public static void Warning(string message) 
        {
            if (!ShowLogs) return;
            Debug.LogWarning($"{LogPrefix}{WarningPrefix}{message}");
        }
        
        
        /// <summary>
        /// Displays a error debug message for the build versions asset...
        /// </summary>
        /// <param name="message">The message to show...</param>
        public static void Error(string message)
        {
            if (!ShowLogs) return;
            Debug.LogError($"{LogPrefix}{ErrorPrefix}{message}");
        }
    }
}