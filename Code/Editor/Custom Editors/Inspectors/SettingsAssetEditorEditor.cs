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
    [CustomEditor(typeof(SettingsAssetEditor))]
    public sealed class SettingsAssetEditorEditor : UnityEditor.Editor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public override void OnInspectorGUI()
        {
            DrawHeaderSection();
            
            GUILayout.Space(5f);

            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(2.5f);
            
            UtilEditor.DrawSoScriptSection(target);
            
            GUILayout.Space(12.5f);
            
            EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("saveEditorTabPos"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("backgroundColor"));

            GUILayout.Space(5f);
            
            EditorGUILayout.LabelField("Save Profile Generator", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lastProfileName"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("saveEditorProfileCreator"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("saveEditorProfileViewer"));
            
            GUILayout.Space(5f);
            
            EditorGUILayout.LabelField("Save Object Generator", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lastSaveObjectName"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lastSaveObjectFileName"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("justCreatedSaveObject"));
            
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static void DrawHeaderSection()
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
    }
}