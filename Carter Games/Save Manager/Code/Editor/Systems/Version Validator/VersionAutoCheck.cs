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
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
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
            
            if (!PerUserSettings.VersionValidationAutoCheckOnLoad) return;
            AutoVersionCheckInit();
        }
        
        
        /// <summary>
        /// Runs the version check logic and listens for its response.
        /// </summary>
        private static void AutoVersionCheckInit()
        {
            VersionChecker.GetLatestVersions();
            VersionChecker.ResponseReceived.Add(OnVersionCheckResponse);
            VersionChecker.ErrorReceived.Add(VersionEditorGUI.ShowErrorDialogue);
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
