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

namespace CarterGames.Shared.SaveManager.Editor
{
    /// <summary>
    /// Handles the auto update checker for the asset.
    /// </summary>
    [InitializeOnLoad]
    public sealed class VersionAutoCheck
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The key for the session pref... should be per asset based on the version key to check...
        /// </summary>
        private static readonly string AutoVersionCheckSessionInitKey = $"{VersionInfo.Key.Trim()}_Editor_Settings_AutoVersionCheckRan";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Runs when the editor is opened or needs to reload.
        /// </summary>
        static VersionAutoCheck()
        {
            // Ensures that this logic only runs once per editor use, so it doesn't appear when they make a code change etc.
            if (SessionState.GetBool(AutoVersionCheckSessionInitKey, false)) return;
            EditorApplication.delayCall += OnEditorLoad;
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Runs when the editor has loaded to check for the latest version of the asset.
        /// </summary>
        private static void OnEditorLoad()
        {
            EditorApplication.delayCall -= OnEditorLoad;
            SessionState.SetBool(AutoVersionCheckSessionInitKey, true);
            
            if (!SharedPerUserSettings.VersionValidationAutoCheckOnLoad) return;
            AutoVersionCheckInit();
        }
        
        
        /// <summary>
        /// Runs the version check logic and listens for its response.
        /// </summary>
        private static void AutoVersionCheckInit()
        {
            VersionChecker.GetLatestVersions();
            VersionChecker.ResponseReceived.Add(OnVersionCheckResponse);
        }


        /// <summary>
        /// Runs when the response is received and only shows a new version is available, not if its on the latest.
        /// </summary>
        private static void OnVersionCheckResponse()
        {
            VersionChecker.ResponseReceived.Remove(OnVersionCheckResponse);
            VersionEditorGUI.ShowResponseDialogue(false);
        }
    }
}
