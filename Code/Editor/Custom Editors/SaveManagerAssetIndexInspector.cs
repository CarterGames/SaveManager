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
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the custom inspector for the Save Manager asset index.
    /// </summary>
    [CustomEditor(typeof(SmDataAssetIndex))]
    public sealed class SaveManagerAssetIndexEditor : UnityEditor.Editor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private Dictionary<string, int> entryLookup = new Dictionary<string, int>();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnEnable()
        {
            entryLookup ??= new Dictionary<string, int>();
            entryLookup?.Clear();

            if (serializedObject.Fp("assets").Fpr("list").arraySize <= 0) return;
            
            for (var i = 0; i < serializedObject.Fp("assets").Fpr("list").arraySize; i++)
            {
                entryLookup.Add(serializedObject.Fp("assets").Fpr("list").GetIndex(i).Fpr("key").stringValue, i);
            }
        }


        public override void OnInspectorGUI()
        {
            GUILayout.Space(5f);
            
            EditorGUILayout.BeginVertical("HelpBox");
            
            GUILayout.Space(2.5f);
            UtilEditor.DrawSoScriptSection((SmDataAssetIndex) target);
            GUILayout.Space(2.5f);
            
            EditorGUILayout.EndVertical();
            
            GUILayout.Space(5f);
            
            DrawAllReferencesSection();
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Draws the all references GUI.
        /// </summary>
        private void DrawAllReferencesSection()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("All References", EditorStyles.boldLabel);
            UtilEditor.DrawHorizontalGUILine();

            EditorGUI.indentLevel++;
            
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            EditorGUILayout.PropertyField(serializedObject.Fp("assets").Fpr("list"));
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.indentLevel--;
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }
    }
}