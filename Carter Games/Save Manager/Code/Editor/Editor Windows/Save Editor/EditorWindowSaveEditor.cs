/*
 * Save Manager
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

using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class EditorWindowSaveEditor : EditorWindow
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private SaveEditorGlobalTab globalTab;
        private SaveEditorSlotsTab slotTab;
        private SaveEditorCapturesTab capturesTab;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private int CurrentTab
        {
            get => EditorUserSettings.GetInt("cg_sm_editor_window_tab");
            set => EditorUserSettings.SetInt("cg_sm_editor_window_tab", value);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Window Access Method
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [MenuItem("Tools/Carter Games/Save Manager/Save Editor", priority = 15)]
        private static void ShowWindow()
        {
            var window = GetWindow<EditorWindowSaveEditor>();
            window.titleContent = new GUIContent("Save Editor", EditorArtHandler.GetIcon(SaveManagerConstants.WindowIcon));
            window.Show();
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("HelpBox");

            if (GUILayout.Button("Global Data", GUILayout.Height(25)))
            {
                CurrentTab = 0;
            }

            EditorGUI.BeginDisabledGroup(!ScriptableRef.GetAssetDef<DataAssetSettings>().DataAssetRef.UseSaveSlots);
            if (GUILayout.Button("Save Slots", GUILayout.Height(25)))
            {
                CurrentTab = 1;
            }
            EditorGUI.EndDisabledGroup();
            
            if (GUILayout.Button("Save Captures", GUILayout.Height(25)))
            {
                CurrentTab = 2;
            }
            
            EditorGUILayout.EndHorizontal();

            if (CurrentTab == 1 && !ScriptableRef.GetAssetDef<DataAssetSettings>().DataAssetRef.UseSaveSlots)
            {
                CurrentTab = 0;
            }
            
            switch (CurrentTab)
            {
                case 0:
                    globalTab ??= new SaveEditorGlobalTab();
                    globalTab.DrawGUI();
                    break;
                case 1:
                    slotTab ??= new SaveEditorSlotsTab();
                    slotTab.DrawGUI();
                    break;
                case 2:
                    capturesTab ??= new SaveEditorCapturesTab();
                    capturesTab.DrawGUI();
                    break;
            }
            
            
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Reset Save", GUILayout.Height(25)))
            {
                var result = EditorUtility.DisplayDialog("Reset Save",
                    "Are you sure you want to reset your save data, this cannot be undone one applied", "Reset Save",
                    "Cancel");

                if (!result) return;

                EditorSaveObjectController.ResetAllObjects();
            }
        }
    }
}