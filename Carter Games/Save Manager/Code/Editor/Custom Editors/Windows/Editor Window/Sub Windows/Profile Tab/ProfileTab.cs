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

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor.SubWindows
{
    /// <summary>
    /// Draws the profile tab of the save editor.
    /// </summary>
    public sealed class ProfileTab
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        // Help box dialogue.
        private const string NoSaveObjectsDialogue =
            "You don't have any save objects setup, so this tool is disabled. Create & add a save object implementation to use the save profiles tab.";
        
        // Fields
        private static Rect deselectRect;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public void DrawTab(List<SaveObject> saveObjects)
        {
            if (saveObjects.Count <= 0)
            {
                // No save objects, so disable.
                EditorGUILayout.HelpBox(NoSaveObjectsDialogue, MessageType.Info);
            }
            else
            {
                PerUserSettings.SaveProfileTabScrollRectPos = EditorGUILayout.BeginScrollView(PerUserSettings.SaveProfileTabScrollRectPos);
                
                EditorGUILayout.BeginVertical("HelpBox");
                SaveProfileCreatorGUI.DrawDisplay();
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.BeginVertical("HelpBox");
                SaveProfileListGUI.DrawDisplay();
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.Space(10);
                EditorGUILayout.EndScrollView();
            }
            
            
            // Draws a deselect box around the rest of the GUI.
            UtilEditor.CreateDeselectZone(ref deselectRect);
        }
    }
}