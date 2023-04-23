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

using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// A custom inspector for the settings asset scriptable object.
    /// </summary>
    [CustomEditor(typeof(SettingsAssetRuntime))]
    public sealed class SettingsAssetRuntimeEditor : UnityEditor.Editor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public override void OnInspectorGUI()
        {
            DrawCogIcon();
            
            GUILayout.Space(5f);
            
            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.Space(1f);
            
            DrawDataSettings();
            DrawOptionsSettings();
            
            EditorGUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
            
            GUILayout.Space(2.5f);
            DrawEditSettingsButton();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        /// <summary>
        /// Draws the cog logo when called.
        /// </summary>
        private static void DrawCogIcon()
        {
            GUILayout.Space(5f);
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (UtilEditor.CogIcon != null)
            {
                if (GUILayout.Button(UtilEditor.CogIcon, GUIStyle.none, GUILayout.MaxHeight(75)))
                {
                    GUI.FocusControl(null);
                }
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(5f);
        }


        /// <summary>
        /// Draws the data settings section when called.
        /// </summary>
        private void DrawDataSettings()
        {
            UtilEditor.DrawSoScriptSection(target as SettingsAssetRuntime);

            GUILayout.Space(12.5f);

            EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultSavePath"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultSavePathWeb"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("saveDataAsset"));
            
            EditorGUI.EndDisabledGroup();
        }


        /// <summary>
        /// Draws the options settings when called.
        /// </summary>
        private void DrawOptionsSettings()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("encryptionOption"), new GUIContent("Encryption Option"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("prettify"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("autoLoadOnEntry"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("autoSaveOnExit"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("showLogs"));
            EditorGUI.EndDisabledGroup();
        }


        /// <summary>
        /// Draws the editor settings button when called.
        /// </summary>
        private static void DrawEditSettingsButton()
        {
            GUI.backgroundColor = UtilEditor.Green;
            
            if (GUILayout.Button("Edit Settings", GUILayout.Height(25f)))
            {
                SettingsService.OpenProjectSettings(UtilEditor.SettingsWindowPath);
            }
            
            GUI.backgroundColor = UtilEditor.SettingsAssetEditor.BackgroundColor;
        }
    }
}