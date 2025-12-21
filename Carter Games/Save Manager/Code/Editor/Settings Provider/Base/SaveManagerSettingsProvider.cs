/*
 * Copyright (c) 2025 Carter Games
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