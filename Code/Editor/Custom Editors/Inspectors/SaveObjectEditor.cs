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

using System;
using System.Collections.Generic;
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
        private Dictionary<string, SerializedProperty> propertiesLookup;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnEnable()
        {
            targetSaveObject = target as SaveObject;

            if (targetSaveObject == null) return;
            if (!targetSaveObject!.IsInitialized) return;
            
            propertiesLookup = new Dictionary<string, SerializedProperty>()
            {
                { "SaveKey", serializedObject.FindProperty("saveKey") },
            };
        }


        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space(7.5f);
            
            InitializeObject();
            
            UtilEditor.DrawHorizontalGUILine();
            
            // Checks for changes on this save object.
            EditorGUI.BeginChangeCheck();
            
            DrawInfoSection();
            EditorGUILayout.Space(3.5f);
            DrawValuesSection();
            
            
            // Applies changes only if there are changes made.
            if (!EditorGUI.EndChangeCheck()) return;
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Initializes the save object via a button press.
        /// </summary>
        private void InitializeObject()
        {
            if (targetSaveObject.IsInitialized) return;
            
            if (GUILayout.Button("Initialize Save Object"))
            {
                serializedObject.FindProperty("saveKey").stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();

                // Adds to save data if it doesn't exist.
                if (UtilEditor.Settings.SaveData.Data.Contains((SaveObject)target)) return;
                
                UtilEditor.Settings.SaveData.Data.Add((SaveObject)target);
                        
                propertiesLookup = new Dictionary<string, SerializedProperty>()
                {
                    { "SaveKey", serializedObject.FindProperty("saveKey") },
                };
                
                EditorUtility.SetDirty(UtilEditor.Settings.SaveData);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                SaveManagerEditorWindow.SaveObjectAddedToSaveData.Raise();

                Repaint();
            }
        }

        
        /// <summary>
        /// Draws the info section of the save object editor.
        /// </summary>
        private void DrawInfoSection()
        {
            if (!targetSaveObject.IsInitialized) return;
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(1f);

            UtilEditor.DrawSoScriptSection(target);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("saveKey"));

            EditorGUILayout.Space(1f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the values section for the save object.
        /// </summary>
        private void DrawValuesSection()
        {
            if (!targetSaveObject.IsInitialized) return;
            
            EditorGUILayout.BeginVertical();
            UtilEditor.DrawHorizontalGUILine();
            
            var prop = serializedObject.GetIterator();

            if (prop.NextVisible(true))
            {
                while (prop.NextVisible(false))
                {
                    if (propertiesLookup.ContainsKey(prop.name)) continue;

                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                }
            }
            
            EditorGUILayout.Space(1f);
            EditorGUILayout.EndVertical();
        }
    }
}