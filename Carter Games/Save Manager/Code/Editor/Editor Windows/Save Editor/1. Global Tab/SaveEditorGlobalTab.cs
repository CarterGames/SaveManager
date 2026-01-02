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
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the GUI for the global tab in the save editor window.
    /// </summary>
    public sealed class SaveEditorGlobalTab
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private Dictionary<string, IEnumerable<SaveObject>> categoriesLookup;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private Vector2 ScrollPos
        {
            get => EditorUserSettings.GetVec2("cg_sm_global_scroll_pos");
            set => EditorUserSettings.SetVec2("cg_sm_global_scroll_pos", value);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public void DrawGUI()
        {
            // Don't load if not initialized.
            if (!EditorSaveObjectController.IsInitialized) return;
            
            // If no slots, show there is no data
            if (!EditorSaveObjectController.HasGlobalSaveObjects)
            {
                EditorGUILayout.HelpBox("No global Save Objects found in the project.", MessageType.Info);
                return;
            }
            
            EditorGUI.BeginChangeCheck();

            if (categoriesLookup == null)
            {
                categoriesLookup = new Dictionary<string, IEnumerable<SaveObject>>();
                var data = EditorSaveObjectController.GlobalSaveObjects;

                foreach (var category in SaveCategoryAttributeHelper.GetCategoryNames(data))
                {
                    categoriesLookup.Add(category, SaveCategoryAttributeHelper.GetObjectsInCategory(data, category));
                }
                
                categoriesLookup.Add(string.Empty, EditorSaveObjectController.GlobalSaveObjects.Where(t => categoriesLookup.Values.All(x => !x.Contains(t))));
            }

            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);

            if (categoriesLookup.ContainsKey("Uncategorized"))
            {
                foreach (var saveObject in categoriesLookup["Uncategorized"])
                {
                    if (!EditorSaveObjectController.TryGetEditorForObject(saveObject, out var editor)) continue;
                    EditorSaveObjectGUI.DrawSaveObjectEditor(saveObject, editor);
                }
            }

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Categories", EditorStyles.boldLabel);
            UtilEditor.DrawHorizontalGUILine();
            
            foreach (var entry in categoriesLookup)
            {
                if (entry.Key == string.Empty) continue;
                if (entry.Key == "Uncategorized") continue;

                EditorGUI.BeginChangeCheck();
                
                var foldout = EditorGUILayout.Foldout(SaveCategoryAttributeHelper.IsCategoryExpanded(entry.Key),
                    entry.Key);

                if (EditorGUI.EndChangeCheck())
                {
                    SaveCategoryAttributeHelper.SetIsCategoryExpanded(entry.Key, foldout);
                }
                
                if (!SaveCategoryAttributeHelper.IsCategoryExpanded(entry.Key)) continue;
                
                foreach (var saveObject in entry.Value)
                {
                    if (!EditorSaveObjectController.TryGetEditorForObject(saveObject, out var editor)) continue;
                    EditorSaveObjectGUI.DrawSaveObjectEditor(saveObject, editor);
                }
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}