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
    /// A custom inspector for the settings asset scriptable object.
    /// </summary>
    [CustomEditor(typeof(DataAssetSettings))]
    public sealed class SettingsAssetRuntimeEditor : UnityEditor.Editor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public override void OnInspectorGUI()
        {
            DrawCogIcon();
            
            GUILayout.Space(5f);

            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
            
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

            if (GUILayout.Button(EditorArtHandler.GetIcon(SaveManagerConstants.CogIcon), GUIStyle.none, GUILayout.MaxHeight(75)))
            {
                GUI.FocusControl(null);
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
            UtilEditor.DrawSoScriptSection(target as DataAssetSettings);

            GUILayout.Space(12.5f);

            EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.PropertyField(serializedObject.Fp("defaultSavePath"));
            EditorGUILayout.PropertyField(serializedObject.Fp("defaultSavePathWeb"));
            EditorGUILayout.PropertyField(serializedObject.Fp("saveDataAsset"));
            
            EditorGUI.EndDisabledGroup();
        }


        /// <summary>
        /// Draws the options settings when called.
        /// </summary>
        private void DrawOptionsSettings()
        {
            EditorGUI.BeginDisabledGroup(true);
            // EditorGUILayout.PropertyField(serializedObject.Fp("encryptionOption"), new GUIContent("Encryption Option"));
            EditorGUILayout.PropertyField(serializedObject.Fp("prettify"));
            EditorGUILayout.PropertyField(serializedObject.Fp("autoSaveOnExit"));
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
            
            GUI.backgroundColor = Color.white;
        }
    }
}