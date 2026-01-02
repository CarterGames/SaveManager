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

using System.Collections.Generic;
using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;
using SettingsProvider = UnityEditor.SettingsProvider;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static partial class SaveManagerSettingsProvider
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        // References
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static SettingsProvider provider;
        private static Rect deselectRect;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the settings asset in the project as a SerializedObject.
        /// </summary>
        private static SerializedObject SettingsAssetObject => ScriptableRef.GetAssetDef<DataAssetSettings>().ObjectRef;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Menu Items
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The menu item for opening the settings window.
        /// </summary>
        [MenuItem("Tools/Carter Games/Save Manager/Edit Settings", priority = 0)]
        public static void OpenSettings()
        {
            SettingsService.OpenProjectSettings("Carter Games/Assets/Save Manager");
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Handles the settings window in the engine.
        /// </summary>
        [SettingsProvider]
        public static SettingsProvider DrawSettingsProvider()
        {
            var provider = new SettingsProvider(UtilEditor.SettingsWindowPath, SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Space(10f);
                    
                    DrawInfo();
                    EditorGUILayout.Space(7.5f);
                    
                    EditorGUI.BeginChangeCheck();
                    
                    DrawEditorSettings();
                    EditorGUILayout.Space(7.5f);
                    DrawSaveSettings();
                    EditorGUILayout.Space(7.5f);
                    DrawSaveSlotSettings();
                    EditorGUILayout.Space(7.5f);
                    DrawSaveMetaSettings();
                    EditorGUILayout.Space(7.5f);
                    DrawSaveBackupSettings();
                    EditorGUILayout.Space(7.5f);
                    DrawSaveEncryptionSettings();
                    EditorGUILayout.Space(7.5f);
                    DrawLegacySaveSettings();
                    EditorGUILayout.Space(7.5f);
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        SettingsAssetObject.ApplyModifiedProperties();
                        SettingsAssetObject.Update();
                    }
                    
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(1.5f);
                    
                    EditorGUILayout.LabelField("Useful Links", EditorStyles.boldLabel);
                    GUILayout.Space(1.5f);
                    
                    DrawButtons();
                    
                    GUILayout.Space(2.5f);
                    EditorGUILayout.EndVertical();
                    
                    UtilEditor.CreateDeselectZone(ref deselectRect);
                    EditorGUI.indentLevel--;
                },
                
                keywords = new HashSet<string>(new[]
                {
                    "Carter Games", "External Assets", "Tools", "Save Manager", "Save", "Game Save Management"
                })
            };
            
            return provider;
        }
    }
}