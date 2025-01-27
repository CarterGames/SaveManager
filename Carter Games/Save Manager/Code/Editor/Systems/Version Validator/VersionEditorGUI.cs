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
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// A helper class for using the version system on editor.
    /// </summary>
    public static class VersionEditorGUI
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Draws a check for updates button when called with dialogues to show the results.
        /// </summary>
        public static void DrawCheckForUpdatesButton()
        {
            if (VersionChecker.IsChecking)
            {
                GUI.backgroundColor = Color.gray;
                EditorGUILayout.LabelField("Checking...", new GUIStyle("minibutton"), GUILayout.MaxWidth(135));
                GUI.backgroundColor = Color.white;
            }
            else
            {
                if (GUILayout.Button("Check For Updates", GUILayout.MaxWidth(135)))
                {
                    VersionChecker.GetLatestVersions();
            
                    VersionChecker.ResponseReceived.AddAnonymous("versionCheckManual", () => ShowResponseDialogue());
                    VersionChecker.ErrorReceived.Add(ShowErrorDialogue);
                }
            }
        }
        
        
        /// <summary>
        /// Shows the response to a version check call as a dialogue box.
        /// </summary>
        /// <param name="showIfUptoDate">Should the box appear if the version is upto date?</param>
        public static void ShowResponseDialogue(bool showIfUptoDate = true)
        {
            VersionChecker.ResponseReceived.RemoveAnonymous("versionCheckManual");
            
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


        /// <summary>
        /// Shows an error dialogue when an error is received instead of data.
        /// </summary>
        public static void ShowErrorDialogue()
        {
            VersionChecker.ErrorReceived.Remove(ShowErrorDialogue);
            
            EditorUtility.DisplayDialog("Update Checker",
                "Unable to check at this time, please try again later.",
                "Continue");
        }
    }
}