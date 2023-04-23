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
    /// A custom inspector for the encryption key asset.
    /// </summary>
    [CustomEditor(typeof(EncryptionKeyAsset))]
    public sealed class EncryptionKeyEditor : UnityEditor.Editor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private const string DialogueTitle = "Generate Encryption Key";
        private const string DialogueDesc = "Are you sure you want to change the encryption key? this process cannot be reversed without source control once performed.";
        private const string DialogueAccept = "Generate";
        private const string DialogueCancel = "Cancel";

        private EncryptionKeyAsset keyAssetValue;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets if there is a key already.
        /// </summary>
        private bool HasKey => keyAssetValue.SaveEncryptionKey != null && keyAssetValue.SaveEncryptionKey.Length > 0;
        
        
        /// <summary>
        /// Gets the string for the generate button based on if there is a key or not.
        /// </summary>
        private string GenerateButtonString => HasKey ? "Re-generate" : "Generate";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnEnable()
        {
            keyAssetValue = target as EncryptionKeyAsset;
        }


        public override void OnInspectorGUI()
        {
            DrawHeaderSection();
            
            GUILayout.Space(5f);

            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(2.5f);

            
            // Draws the script field.
            /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
            UtilEditor.DrawSoScriptSection(target);

            
            // Draws the has key label.
            /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Has Key:", EditorStyles.boldLabel, GUILayout.Width(52.5f));

            GUI.color = HasKey ? UtilEditor.Green : UtilEditor.Red;
            EditorGUILayout.LabelField((HasKey).ToString());
            GUI.color = UtilEditor.SettingsAssetEditor.BackgroundColor;
            
            EditorGUILayout.EndHorizontal();

            
            // Draws the generate new key button.
            /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
            GUILayout.Space(2.5f);

            if (GUILayout.Button(GenerateButtonString))
            {
                if (EditorUtility.DisplayDialog(DialogueTitle, DialogueDesc, DialogueAccept, DialogueCancel))
                {
                    GenerateKey();
                }
            }
            
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Draws the header section of the editor.
        /// </summary>
        private static void DrawHeaderSection()
        {
            GUILayout.Space(5f);
                    
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
                    
            if (UtilEditor.KeyIcon != null)
            {
                if (GUILayout.Button(UtilEditor.KeyIcon, GUIStyle.none, GUILayout.MaxHeight(75)))
                {
                    GUI.FocusControl(null);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
                    
            GUILayout.Space(5f);
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Utility Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Generates a encryption key when called.
        /// </summary>
        private void GenerateKey()
        {
            var key = keyAssetValue.GenerateKey();
                    
            if (serializedObject.FindProperty("saveEncryptionKey") != null)
            {
                serializedObject.FindProperty("saveEncryptionKey").ClearArray();
            }

            for (var i = 0; i < key.Length; i++)
            {
                serializedObject.FindProperty("saveEncryptionKey").InsertArrayElementAtIndex(i);
                serializedObject.FindProperty("saveEncryptionKey").GetArrayElementAtIndex(i).intValue = key[i];
            }
                    
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            EditorUtility.SetDirty(target);
                    
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}