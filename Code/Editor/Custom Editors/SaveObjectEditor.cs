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

using System;
using System.Collections.Generic;
using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the custom inspector for save objects.
    /// </summary>
    [CustomEditor(typeof(SaveObject), true)]
    public sealed class SaveObjectEditor : UnityEditor.Editor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private SaveObject targetSaveObject;
        private Dictionary<string, SerializedProperty> propertiesLookup = new Dictionary<string, SerializedProperty>();

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnEnable()
        {
            targetSaveObject = target as SaveObject;

            if (targetSaveObject == null) return;
            if (!targetSaveObject) return;
            
            propertiesLookup = new Dictionary<string, SerializedProperty>()
            {
                { "SaveKey", serializedObject.Fp("saveKey") },
            };
        }


        public void EditorWindowGUI()
        {
            InitializeObject();
            
            DrawValuesSection();
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Initializes the save object via a button press.
        /// </summary>
        private void InitializeObject()
        {
            if (targetSaveObject) return;

            serializedObject.Fp("saveKey").stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            propertiesLookup = new Dictionary<string, SerializedProperty>()
            {
                {"SaveKey", serializedObject.Fp("saveKey")},
            };

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Repaint();
        }


        /// <summary>
        /// Draws the values section for the save object.
        /// </summary>
        private void DrawValuesSection()
        {
            EditorGUILayout.BeginVertical();
            UtilEditor.DrawHorizontalGUILine();
            
            var prop = serializedObject.GetIterator();
            
            if (prop.NextVisible(true))
            {
                while (prop.NextVisible(false))
                {
                    if (propertiesLookup.ContainsKey(prop.name)) continue;
                    if (prop.type != "SaveValue`1") continue;

                    EditorGUI.BeginChangeCheck();
                    
                    EditorGUILayout.PropertyField(serializedObject.Fp(prop.name), true);

                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                        serializedObject.Update();
                        
                        EditorSaveManager.TrySetDirty();
                    }
                }
            }
            
            EditorGUILayout.Space(.5f);
            EditorGUILayout.EndVertical();
        }
    }
}