/*
 * Save Manager
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
    /// <summary>
    /// A helper class for using the version system on editor.
    /// </summary>
    public static class VersionEditorGUI
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Defines if a request is currently running.
        /// </summary>
        private static bool RequestInProgress { get; set; }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Draws a check for updates button when called with dialogues to show the results.
        /// </summary>
        public static void DrawCheckForUpdatesButton()
        {
            EditorGUI.BeginDisabledGroup(RequestInProgress);
            
            if (!GUILayout.Button("Check For Updates"))
            {
                EditorGUI.EndDisabledGroup();
                return;
            }
            
            EditorGUI.EndDisabledGroup();

            RequestInProgress = true;
            VersionChecker.GetLatestVersions();
            
            VersionChecker.ResponseReceived.AddAnonymous("versionCheckManual", () => ShowResponseDialogue());
            VersionChecker.ErrorReceived.Add(ShowErrorDialogue);
        }
        
        
        /// <summary>
        /// Shows the response to a version check call as a dialogue box.
        /// </summary>
        /// <param name="showIfUptoDate">Should the box appear if the version is upto date?</param>
        public static void ShowResponseDialogue(bool showIfUptoDate = true)
        {
            VersionChecker.ResponseReceived.RemoveAnonymous("versionCheckManual");
            VersionChecker.ErrorReceived.Remove(ShowErrorDialogue);

            RequestInProgress = false;
            
            if (VersionChecker.IsNewerVersion)
            {
                if (!showIfUptoDate) return;
                EditorUtility.DisplayDialog("Update Checker",
                    $"You are using a newer version than the currently released one.\n\nYours: {VersionInfo.ProjectVersionNumber}\nLatest: {VersionChecker.LatestVersionNumberString}",
                    "Continue");
            }
            else if (!VersionChecker.IsLatestVersion)
            {
                if (InstallMethodChecker.IsPackageInstalled)
                {
                    EditorUtility.DisplayDialog("Update Checker",
                            $"You are using an older version of this package.\n\nCurrent: {VersionInfo.ProjectVersionNumber}\nLatest: {VersionChecker.LatestVersionNumberString}\n\nYou can get the latest release from the package manager.",
                            "Continue");
                }
                else
                {
                    if (EditorUtility.DisplayDialog("Update Checker",
                            $"You are using an older version of this package.\n\nCurrent: {VersionInfo.ProjectVersionNumber}\nLatest: {VersionChecker.LatestVersionNumberString}",
                            "Latest Release", "Continue"))
                    {
                        Application.OpenURL(VersionChecker.DownloadURL);
                    }
                }
            }
            else
            {
                if (!showIfUptoDate) return;
                
                EditorUtility.DisplayDialog("Update Checker",
                    "You are using the latest version!",
                    "Continue");
            }
        }


        private static void ShowErrorDialogue(VersionPacketError errorPacket)
        {
            VersionChecker.ResponseReceived.RemoveAnonymous("versionCheckManual");
            VersionChecker.ErrorReceived.Remove(ShowErrorDialogue);
            
            RequestInProgress = false;
            
            EditorUtility.DisplayDialog("Update Checker [ERROR]",
                errorPacket.Error,
                "Continue");
        }
    }
}