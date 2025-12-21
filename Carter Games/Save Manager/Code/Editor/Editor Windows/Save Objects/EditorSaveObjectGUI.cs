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
    public static class EditorSaveObjectGUI
    {
        public static void DrawSaveObjectEditor(SaveObject targetSaveObject, SaveObjectEditor editor, int slotIndex = -1)
        {
            if (targetSaveObject == null) return;
            
            // Ignores save objects with nothing to show.
            if (editor.serializedObject.GetIterator().CountRemaining() <= 1)
            {
                return;
            }

            GUIContent soLabelContent;

            editor.serializedObject.Fp("editor_hasWarning").boolValue = HasAnySaveValueIssues(targetSaveObject, editor.serializedObject);
            editor.serializedObject.ApplyModifiedProperties();
            editor.serializedObject.Update();
            
            GUI.backgroundColor = editor.serializedObject.Fp("editor_hasWarning").boolValue 
                    ? Color.yellow 
                    : Color.white;

            if (editor.serializedObject.Fp("editor_hasWarning").boolValue)
            {
                soLabelContent = new GUIContent($" {targetSaveObject.GetType().Name}", EditorArtHandler.GetIcon(SaveManagerConstants.WarningIcon));
            }
            else
            {
                soLabelContent = new GUIContent(targetSaveObject.GetType().Name);
            }
            
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();


            var saved = slotIndex >= 0
                ? EditorTogglesHandler.GetSaveObjectIsExpanded(targetSaveObject, slotIndex)
                : EditorTogglesHandler.GetSaveObjectIsExpanded(targetSaveObject);
            
            EditorGUI.BeginChangeCheck();
            var isExpanded = EditorGUILayout.Foldout(saved, soLabelContent);
            if (EditorGUI.EndChangeCheck())
            {
                if (slotIndex >= 0)
                {
                    EditorTogglesHandler.SetSaveObjectIsExpanded(targetSaveObject, slotIndex, isExpanded);
                }
                else
                {
                    EditorTogglesHandler.SetSaveObjectIsExpanded(targetSaveObject, isExpanded);
                }
            }


            GUILayout.FlexibleSpace();
            
            if (editor.serializedObject.Fp("editor_hasWarning").boolValue)
            {
                EditorGUILayout.HelpBox(editor.serializedObject.Fp("editor_warningMessage").stringValue, MessageType.None);
            }
            

            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            // GUI.backgroundColor = Color.cyan;
            
            // if (GUILayout.Button("Defaults", GUILayout.Width(75)))
            // {
            //     SaveDefaultsWindow.ShowDefaultsWindow(targetSaveObject, SaveManagerEditorCache.SoLookup[targetSaveObject]);
            // }
            
            GUI.backgroundColor = UtilEditor.Red;
            
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Reset Save Object",
                        "Are you sure you want to reset all values on this save object?", "Reset", "Cancel"))
                {
                    // Reset Save Object
                    Undo.RecordObject(targetSaveObject, "Save Object reset to default values");
                    
                    targetSaveObject.ResetObjectSaveValues();

                    editor.serializedObject.ApplyModifiedProperties();
                    editor.serializedObject.Update();

                    // OldSaveManager.Save();
                    GUI.FocusControl(null);

                    return;
                }
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2.5f);
//

            if (isExpanded)
            {
                EditorGUI.BeginChangeCheck();
                
                editor.EditorWindowGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    editor.serializedObject.ApplyModifiedProperties();
                    editor.serializedObject.Update();
                }
                
                GUILayout.Space(1.5f);
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                editor.serializedObject.ApplyModifiedProperties();

                // SaveManager.SaveGame();
                // OldSaveManager.Save();
            }
            
            editor.serializedObject.Update();

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
            
            GUI.backgroundColor = Color.white;

            GUILayout.Space(4f);
        }
        
        
        private static bool HasAnySaveValueIssues(SaveObject saveObject, SerializedObject so)
        {
            var current = so.GetIterator();
            
            if (current.NextVisible(true))
            {
                while (current.NextVisible(false))
                {
                    if (current.type != "SaveValue`1") continue;

                    if (EditorSaveObjectController.IsInitialized)
                    {
                        if (EditorSaveObjectController.HasDuplicateSaveValueKeys(saveObject))
                        {
                            current.serializedObject.Fp("editor_warningMessage").stringValue = "No save key assigned to one or multiple save values, these values will not save until this is corrected.";
                            return true;
                        }
                    }
                    
                    if (string.IsNullOrEmpty(current.Fpr("key").stringValue))
                    {
                        current.serializedObject.Fp("editor_warningMessage").stringValue = "No save key assigned to one or multiple save values, these values will not save until this is corrected.";
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}