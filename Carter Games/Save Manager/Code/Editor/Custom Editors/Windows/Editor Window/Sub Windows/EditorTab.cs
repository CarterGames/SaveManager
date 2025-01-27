/*
 * Copyright (c) 2024 Carter Games
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

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor.SubWindows
{
    public sealed class EditorTab
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private const string DemoSaveObjectFullName = "CarterGames.Assets.SaveManager.Demo.ExampleSaveObject";
        private static Rect deselectRect;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Initialize Method
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public void Initialize() { }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public void DrawTab(List<SaveObject> saveObjects)
        {
            if (saveObjects.Count <= 0) return;

            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox(
                    "You cannot edit the save while in play mode, please exit play mode to edit the save data.",
                    MessageType.Info);
            }

            PerUserSettings.SaveEditorTabScrollRectPos =
                EditorGUILayout.BeginScrollView(PerUserSettings.SaveEditorTabScrollRectPos);

            if (SaveManagerEditorCache.SoLookup.Count > 0)
            {
                DrawSaveObjects();
            }

            EditorGUILayout.Space(5f);
            EditorGUILayout.EndScrollView();
            
            // Force update all elements...
            foreach (var lookup in SaveManagerEditorCache.EditorsLookup)
            {
                if (lookup.Key == null) continue;
                
                lookup.Value.serializedObject.ApplyModifiedProperties();
                lookup.Value.serializedObject.Update();
            }
        }


        /// <summary>
        /// Draws a collection of save objects.
        /// </summary>
        private void DrawSaveObjects()
        {
            if (PerUserSettings.SaveEditorOrganiseByCategory)
            {
                var didHaveUncategorized = false;
                var hasDrawnLine = false;


                // Uncategorized Save Objects
                /* ────────────────────────────────────────────────────────────────────────────────────────────────── */
                foreach (var saveObject in SaveCategoryAttributeHelper.GetObjectsInCategory("Uncategorized"))
                {
                    if (saveObject.GetType().FullName == DemoSaveObjectFullName) continue;
                    DrawSaveObjectEditor(saveObject);
                    didHaveUncategorized = true;
                }


                // Categorized Save Objects
                /* ────────────────────────────────────────────────────────────────────────────────────────────────── */
                foreach (var category in SaveCategoryAttributeHelper.GetCategoryNames())
                {
                    if (category.Equals("Uncategorized")) continue;

                    var saveObjectsInCategory = SaveCategoryAttributeHelper.GetObjectsInCategory(category);

                    if (didHaveUncategorized && !hasDrawnLine && saveObjectsInCategory.Count > 0)
                    {
                        UtilEditor.DrawHorizontalGUILine();
                        hasDrawnLine = true;
                    }
                    
                    EditorGUILayout.LabelField(category, EditorStyles.boldLabel);

                    foreach (var saveObject in saveObjectsInCategory)
                    {
                        if (saveObject.GetType().FullName == DemoSaveObjectFullName) continue;
                        DrawSaveObjectEditor(saveObject);
                    }
                }
            }
            else
            {
                // Non-Categorized - But without demo save object.
                /* ────────────────────────────────────────────────────────────────────────────────────────────────── */
                foreach (var objKp in SaveManagerEditorCache.SoLookup)
                {
                    if (objKp.Key.GetType().FullName == DemoSaveObjectFullName) continue;
                    DrawSaveObjectEditor(objKp.Key);
                }
            }


            // Demo Save Object
            /* ────────────────────────────────────────────────────────────────────────────────────────────────────── */
            var demoSaveObject = SaveManagerEditorCache.SoLookup.FirstOrDefault(t =>
                t.Key.GetType().FullName == DemoSaveObjectFullName).Key;
            
            if (demoSaveObject == null) return;
            
            UtilEditor.DrawHorizontalGUILine();
            EditorGUILayout.LabelField("Asset Demo Save Object", EditorStyles.boldLabel);
            DrawSaveObjectEditor(demoSaveObject);
        }


        private void DrawSaveObjectEditor(SaveObject targetSaveObject)
        {
            if (targetSaveObject == null) return;
            
            EditorGUILayout.BeginVertical(SaveManagerEditorCache.EditorsLookup[targetSaveObject].serializedObject.Fp("isExpanded").boolValue
                ? "HelpBox"
                : "Box");

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            if (targetSaveObject != null)
            {
                SaveManagerEditorCache.EditorsLookup[targetSaveObject].serializedObject.Fp("isExpanded").boolValue =
                    EditorGUILayout.Foldout(SaveManagerEditorCache.EditorsLookup[targetSaveObject].serializedObject.Fp("isExpanded").boolValue,
                        targetSaveObject.name);
            }
            

            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            GUI.backgroundColor = Color.cyan;

            if (GUILayout.Button("Defaults", GUILayout.Width(75)))
            {
                SaveDefaultsWindow.ShowDefaultsWindow(targetSaveObject, SaveManagerEditorCache.SoLookup[targetSaveObject]);
            }
            
            GUI.backgroundColor = UtilEditor.Red;
            
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Reset Save Object",
                        "Are you sure you want to reset all values on this save object?", "Reset", "Cancel"))
                {
                    // Reset Save Object
                    Undo.RecordObject(targetSaveObject, "Save Object reset to default values");
                    
                    targetSaveObject.ResetObjectSaveValues();

                    SaveManagerEditorCache.EditorsLookup[targetSaveObject].serializedObject.ApplyModifiedProperties();
                    SaveManagerEditorCache.EditorsLookup[targetSaveObject].serializedObject.Update();

                    SaveManager.Save();
                    GUI.FocusControl(null);

                    return;
                }
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2.5f);


            if (SaveManagerEditorCache.EditorsLookup[targetSaveObject].serializedObject.Fp("isExpanded").boolValue)
            {
                EditorGUI.BeginChangeCheck();
                SaveManagerEditorCache.EditorsLookup[targetSaveObject].EditorWindowGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    SaveManagerEditorCache.EditorsLookup[targetSaveObject].serializedObject.ApplyModifiedProperties();
                    SaveManagerEditorCache.EditorsLookup[targetSaveObject].serializedObject.Update();
                }
                
                GUILayout.Space(1.5f);
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                SaveManagerEditorCache.EditorsLookup[targetSaveObject].serializedObject.ApplyModifiedProperties();
                SaveManagerEditorCache.EditorsLookup[targetSaveObject].serializedObject.Update();

                SaveManager.Save();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();

            GUILayout.Space(4f);
        }



        public void RepaintAll()
        {
            if (!SaveManagerEditorCache.HasCache)
            {
                SaveManagerEditorCache.RefreshCache();
            }
            
            foreach (var keyPair in SaveManagerEditorCache.EditorsLookup)
            {
                keyPair.Value.serializedObject.Update();
            }
        }
        
        
        public void RefreshEditor()
        {
            if (!SaveManagerEditorCache.HasCache)
            {
                SaveManagerEditorCache.RefreshCache();
            }
            
            foreach (var editor in SaveManagerEditorCache.EditorsLookup.Values.ToArray())
            {
                editor.serializedObject.ApplyModifiedProperties();
                editor.serializedObject.Update();
            }
        }
    }
}