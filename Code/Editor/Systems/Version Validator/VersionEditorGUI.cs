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
            if (!GUILayout.Button("Check For Updates", GUILayout.MaxWidth(135))) return;
            
            VersionChecker.GetLatestVersions();
                
            VersionChecker.ResponseReceived.Add(() =>
            {
                if (VersionChecker.IsNewerVersion)
                {
                    EditorUtility.DisplayDialog("Update Checker",
                        $"You are using a newer version than the currently released one.\n\nYours: {VersionInfo.ProjectVersionNumber}\nLatest: {VersionChecker.LatestVersionNumberString}",
                        "Continue");
                }
                else if (!VersionChecker.IsLatestVersion)
                {
                    if (EditorUtility.DisplayDialog("Update Checker",
                            $"You are using an older version of this package.\n\nCurrent: {VersionInfo.ProjectVersionNumber}\nLatest: {VersionChecker.LatestVersionNumberString}",
                            "Latest Release", "Continue"))
                    {
                        Application.OpenURL(VersionChecker.DownloadURL);
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Update Checker",
                        "You are using the latest version!",
                        "Continue");
                }
            });
        }
    }
}