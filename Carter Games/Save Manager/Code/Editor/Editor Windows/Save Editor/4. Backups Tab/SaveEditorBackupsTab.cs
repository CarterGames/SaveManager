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

using System.Linq;
using CarterGames.Assets.SaveManager.Backups;
using CarterGames.Shared.SaveManager.Editor;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the GUI for the backups tab in the save editor window.
    /// </summary>
    public sealed class SaveEditorBackupsTab
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private Vector2 ScrollPos
        {
            get => EditorUserSettings.GetVec2("cg_sm_backups_scroll_pos");
            set => EditorUserSettings.SetVec2("cg_sm_backups_scroll_pos", value);
        }
        
        
        private JObject[] Backups { get; set; }
        private string PreviewBackupName { get; set; }
        private string PreviewBackupJson { get; set; }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public void DrawGUI()
        {
            // Don't load if not initialized.
            if (!EditorSaveObjectController.IsInitialized) return;
            
            EditorGUILayout.Space(7.5f);
            
            EditorGUILayout.LabelField("Save Backups", EditorStyles.boldLabel);
            EditorGUILayout.Space(1.5f);

            EditorGUILayout.HelpBox("Backups are a backend system that will automatically make a new backup each time your game loads successfully. Below you can view these backups, load them or make captures for them so you don't lose them.", MessageType.Info);
            
            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
            
            if (Backups == null)
            {
                Backups = ScriptableRef.GetAssetDef<DataAssetSettings>().AssetRef.BackupLocation.GetBackups().ToArray();
            }

            if (Backups.Any())
            {
                foreach (var entry in Backups)
                {
                    EditorGUILayout.BeginHorizontal("Box");
                    EditorGUILayout.LabelField($"Backup_{entry["iteration"]}");

                    if (GUILayout.Button("View Backup", GUILayout.Width(110)))
                    {
                        PreviewBackupName = $"Backup_{entry["iteration"]}";
                        PreviewBackupJson = entry["json"].ToString();
                    }
                    
                    if (GUILayout.Button("Load Backup", GUILayout.Width(110)))
                    {
                        SaveBackupManager.LoadBackup(entry["json"]);
                    }
                
                    if (GUILayout.Button("Make Capture From Backup", GUILayout.Width(200)))
                    {
                        SaveCaptureManager.CaptureFromBackup(entry["json"]);
                    }
                    
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No backups currently in the project.", MessageType.Info);
            }
            
            if (!string.IsNullOrEmpty(PreviewBackupJson))
            {
                EditorGUILayout.Space(7.5f);
                EditorGUILayout.LabelField($"Backup Data ({PreviewBackupName}):");
                EditorGUILayout.TextArea(PreviewBackupJson);
            }
            
            EditorGUILayout.EndScrollView();
        }
    }
}