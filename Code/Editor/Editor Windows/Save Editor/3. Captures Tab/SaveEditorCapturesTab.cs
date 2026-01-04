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
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the GUI for the captures tab in the save editor window.
    /// </summary>
    public sealed class SaveEditorCapturesTab
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static readonly GUIContent CaptureNameField = new GUIContent("Capture Name", "Set the name for the new save capture to be called.");
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private Vector2 ScrollPos
        {
            get => EditorUserSettings.GetVec2("cg_sm_captures_scroll_pos");
            set => EditorUserSettings.SetVec2("cg_sm_captures_scroll_pos", value);
        }
        
        
        private string CaptureName { get; set; }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public void DrawGUI()
        {
            // Don't load if not initialized.
            if (!EditorSaveObjectController.IsInitialized) return;
            
            EditorGUILayout.Space(7.5f);
            
            EditorGUILayout.LabelField("Create captures", EditorStyles.boldLabel);
            EditorGUILayout.Space(1.5f);

            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);

            EditorGUILayout.BeginHorizontal();

            CaptureName = EditorGUILayout.TextField(CaptureNameField, CaptureName);
            
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(CaptureName));
            if (GUILayout.Button("Capture Current Editor Save", GUILayout.Width(200)))
            {
                SaveCaptureManager.CaptureCurrentEditorSave(CaptureName);
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(10f);
            
            EditorGUILayout.LabelField("Load captures", EditorStyles.boldLabel);
            EditorGUILayout.Space(1.5f);

            if (SaveCaptureManager.TryGetAllCaptures(out var captures))
            {
                EditorGUILayout.BeginVertical();
                
                foreach (var entry in captures)
                {
                    EditorGUILayout.BeginHorizontal("Box");
                    EditorGUILayout.LabelField(entry.CaptureName);

                    if (GUILayout.Button("Select File", GUILayout.Width(100)))
                    {
                        EditorGUIUtility.PingObject(entry.CaptureFile);
                    }
                    
                    if (GUILayout.Button("Load File", GUILayout.Width(100)))
                    {
                        try
                        {
                            SaveCaptureManager.LoadCapture(entry.CaptureFile);
                            EditorUtility.DisplayDialog("Save Capture", $"{entry.CaptureName} loaded successfully.",
                                "Ok");
                        }
#pragma warning disable 0168
                        catch (Exception e)
#pragma warning restore 0168
                        {
                            EditorUtility.DisplayDialog("Save Capture", $"{entry.CaptureName} could not be loaded.",
                                "Ok");
                            
                            SmDebugLogger.LogWarning(SaveManagerErrorCode.SaveCaptureLoadFailed.GetErrorMessageFormat());
                        }
                    }

                    GUI.backgroundColor = Color.red;
                    
                    if (GUILayout.Button("-", GUILayout.Width(25f)))
                    {
                        if (EditorUtility.DisplayDialog("Delete Capture", $"Are you sure you want to delete the {entry.CaptureName} capture?",
                                "Delete", "Cancel"))
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(entry.CaptureFile));
                            AssetDatabase.Refresh();
                        }
                    }
                    
                    GUI.backgroundColor = Color.white;
                    
                    EditorGUILayout.EndHorizontal(); 
                }
                
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.HelpBox("No captures currently in the project.", MessageType.Info);
            }
            
            EditorGUILayout.EndScrollView();
        }
    }
}