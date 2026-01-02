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

using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// The primary utility class for the save manager's editor logic. Should only be used in the editor space!
    /// </summary>
    public static class UtilEditor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        // Paths
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        public const string SettingsWindowPath = "Carter Games/Assets/Save Manager";


        // Colours
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Green Apple (Green) Color
        /// </summary>
        /// <see cref="https://www.computerhope.com/cgi-bin/htmlcolor.pl?c=4CC417"/>
        public static readonly Color Green = new Color32(76, 196, 23, 255);

        
        /// <summary>
        /// Rubber Ducky Yellow (Yellow) Color
        /// </summary>
        /// <see cref="https://www.computerhope.com/cgi-bin/htmlcolor.pl?c=FFD801"/>
        public static readonly Color Yellow = new Color32(255, 216, 1, 255);
        

        /// <summary>
        /// Scarlet Red (Red) Color
        /// </summary>
        /// <see cref="https://www.computerhope.com/cgi-bin/htmlcolor.pl?c=FF2400"/>
        public static readonly Color Red = new Color32(255, 36, 23, 255);
        
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Draws the script fields in the custom inspector...
        /// </summary>
        public static void DrawSoScriptSection(object target)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script:", MonoScript.FromScriptableObject((ScriptableObject)target),
                typeof(object), false);
            EditorGUI.EndDisabledGroup();
        }


        /// <summary>
        /// Draws a horizontal line
        /// </summary>
        public static void DrawHorizontalGUILine()
        {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = new Texture2D(1, 1);
            boxStyle.normal.background.SetPixel(0, 0, Color.grey);
            boxStyle.normal.background.Apply();

            var one = EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", boxStyle, GUILayout.ExpandWidth(true), GUILayout.Height(2));
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Creates a deselect zone to let users click outside of any editor window to unfocus from their last selected field.
        /// </summary>
        /// <param name="rect">The rect to draw on.</param>
        public static void CreateDeselectZone(ref Rect rect)
        {
            if (rect.width <= 0)
            {
                rect = new Rect(0, 0, Screen.width, Screen.height);
            }

            if (GUI.Button(rect, string.Empty, GUIStyle.none))
            {
                GUI.FocusControl(null);
            }
        }
    }
}