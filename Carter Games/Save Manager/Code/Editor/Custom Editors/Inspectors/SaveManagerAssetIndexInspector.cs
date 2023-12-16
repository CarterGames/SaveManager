using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the custom inspector for the Save Manager asset index.
    /// </summary>
    [CustomEditor(typeof(AssetIndex))]
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
            UtilEditor.DrawSoScriptSection((AssetIndex) target);
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
            EditorGUILayout.PropertyField(serializedObject.Fp("assets"));
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.indentLevel--;
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }
    }
}