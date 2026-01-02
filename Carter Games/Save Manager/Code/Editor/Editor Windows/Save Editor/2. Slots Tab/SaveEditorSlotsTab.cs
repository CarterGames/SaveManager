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
    /// Handles the GUI for the slots tab in the save editor window.
    /// </summary>
    public sealed class SaveEditorSlotsTab
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string SlotExpandedKey = "cg_sm_slot_{0}_expanded";
        private const string SlotSaveDataExpandedKey = "cg_sm_slot_{0}_expanded_save_data";
        private Dictionary<string, IEnumerable<SaveObject>> categoriesLookup;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private Vector2 ScrollPos
        {
            get => EditorUserSettings.GetVec2("cg_sm_slots_scroll_pos");
            set => EditorUserSettings.SetVec2("cg_sm_slots_scroll_pos", value);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public void DrawGUI()
        {
            // Don't load if not initialized.
            if (!EditorSaveObjectController.IsInitialized) return;
            
            // If no slots, show there is no data
            if (!EditorSaveObjectController.HasSlotSaveObjects)
            {
                UtilEditor.DrawHorizontalGUILine();
                
                EditorGUILayout.BeginVertical("Box");
                
                if (GUILayout.Button("+ Add Slot To Save", GUILayout.Height(25f)))
                {
                    EditorSlotManager.AddNewSlot();
                    categoriesLookup = null;
                }
                
                EditorGUILayout.EndVertical();
                
                return;
            }
            
            // If no slots defined
            if (EditorSlotManager.TotalSlots <= 0)
            {
                EditorGUILayout.HelpBox("No slots in the save, create one at runtime or here in the editor to see it here.", MessageType.Info);
                DrawCreateSlotGUI();
                return;
            }
            
            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);

            foreach (var entry in EditorSlotManager.AllSlotsData)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.BeginHorizontal();
                
                EditorGUI.BeginChangeCheck();
                var isExpanded = EditorGUILayout.Foldout(EditorTogglesHandler.IsSlotExpanded(SlotExpandedKey, entry.Key), $"Slot {entry.Key}");
                if (EditorGUI.EndChangeCheck())
                {
                    EditorTogglesHandler.SetSlotExpanded(SlotExpandedKey, entry.Key, isExpanded);
                }

                GUILayout.FlexibleSpace();
                GUI.backgroundColor = Color.red;
                
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    if (EditorUtility.DisplayDialog("Delete save slot",
                            "Are you sure you want to remove this save slot?, this cannot be undone once executed.",
                            "Delete Slot", "Cancel"))
                    {
                        // TODO - delete the slot after an editor dialog confirm...
                        EditorSlotManager.DeleteSlot(entry.Value);
                        return;
                    }
                }
                
                GUI.backgroundColor = Color.white;
                
                EditorGUILayout.EndHorizontal();
                
                if (!isExpanded)
                {
                    EditorGUILayout.EndVertical();
                    continue;
                }
                
                UtilEditor.DrawHorizontalGUILine();
                
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField($"Last Saved: {entry.Value.LastSaveDate}");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Total Playtime: {entry.Value.Playtime}");
                EditorGUI.EndDisabledGroup();
                
                GUILayout.FlexibleSpace();
                
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Reset Metadata", GUILayout.Width(120))){}
                GUI.backgroundColor = Color.white;
                
                EditorGUILayout.EndHorizontal();
                
                UtilEditor.DrawHorizontalGUILine();
                
                EditorGUI.BeginChangeCheck();
                var isSlotDataExpanded = EditorGUILayout.Foldout(EditorTogglesHandler.IsSlotSaveDataExpanded(SlotSaveDataExpandedKey, entry.Key), "Save Data");
                if (EditorGUI.EndChangeCheck())
                {
                    EditorTogglesHandler.SetSlotSaveDataExpanded(SlotSaveDataExpandedKey, entry.Key, isSlotDataExpanded);
                }
                
                if (!isSlotDataExpanded)
                {
                    EditorGUILayout.EndVertical();
                    continue;
                }

                DrawSlotSaveData(entry.Key, EditorSaveObjectController.GetSlotSaveObjects(entry.Key));
                
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndScrollView();


            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("+ Add Slot To Save", GUILayout.Height(25f), GUILayout.Width(125f)))
            {
                EditorSlotManager.AddNewSlot();
                categoriesLookup = null;
            }
            
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
        }

        
        private void DrawCreateSlotGUI()
        {
            if (GUILayout.Button("Create save slot"))
            {
                EditorSlotManager.AddNewSlot();
                categoriesLookup = null;
            }
        }


        private void DrawSlotSaveData(int slotKey, IEnumerable<SaveObject> data)
        {
            var actualData = data.ToArray();
            
            if (categoriesLookup == null)
            {
                categoriesLookup = new Dictionary<string, IEnumerable<SaveObject>>();

                foreach (var category in SaveCategoryAttributeHelper.GetCategoryNames(actualData))
                {
                    categoriesLookup.Add(category, SaveCategoryAttributeHelper.GetObjectsInCategory(actualData, category));
                }
                
                categoriesLookup.Add(string.Empty, actualData.Where(t => categoriesLookup.Values.All(x => !x.Contains(t))));
            }
            
            if (categoriesLookup.ContainsKey("Uncategorized"))
            {
                foreach (var saveObject in categoriesLookup["Uncategorized"])
                {
                    if (!EditorSaveObjectController.TryGetEditorForSlotObjectType(slotKey, saveObject.GetType(), out var editor)) continue;
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
                    if (!EditorSaveObjectController.TryGetEditorForSlotObjectType(slotKey, saveObject.GetType(), out var editor)) continue;
                    EditorSaveObjectGUI.DrawSaveObjectEditor(saveObject, editor);
                }
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}