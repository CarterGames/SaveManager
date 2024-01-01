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
        
        private Dictionary<SaveObject, SaveObjectEditor> editorsLookup;
        private Dictionary<SaveObject, SerializedObject> soLookup;

        private static Rect deselectRect;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Initialize Method
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public void Initialize()
        {
            soLookup = new Dictionary<SaveObject, SerializedObject>();
            editorsLookup = new Dictionary<SaveObject, SaveObjectEditor>();
        }

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

            foreach (var saveObj in saveObjects)
            {
                SerializedObject sObj;

                if (soLookup.ContainsKey(saveObj))
                {
                    sObj = soLookup[saveObj];
                }
                else
                {
                    sObj = new SerializedObject(saveObj);
                }


                if (soLookup.ContainsKey(saveObj)) continue;
                soLookup.Add(saveObj, sObj);
            }


            if (soLookup.Count > 0)
            {
                DrawSaveObjects();
            }

            EditorGUILayout.Space(5f);
            EditorGUILayout.EndScrollView();

            // UtilEditor.CreateDeselectZone(ref deselectRect);
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
                        if (saveObject.GetType().FullName == DemoSaveObjectFullName)
                            continue;
                        DrawSaveObjectEditor(saveObject);
                    }
                }
            }
            else
            {
                // Non-Categorized - But without demo save object.
                /* ────────────────────────────────────────────────────────────────────────────────────────────────── */
                foreach (var objKp in soLookup)
                {
                    if (objKp.Key.GetType().FullName == DemoSaveObjectFullName) continue;
                    DrawSaveObjectEditor(objKp.Key);
                }
            }


            // Demo Save Object
            /* ────────────────────────────────────────────────────────────────────────────────────────────────────── */
            var demoSaveObject = soLookup.FirstOrDefault(t =>
                t.Key.GetType().FullName == DemoSaveObjectFullName).Key;
            
            if (demoSaveObject == null) return;
            
            UtilEditor.DrawHorizontalGUILine();
            EditorGUILayout.LabelField("Asset Demo Save Object", EditorStyles.boldLabel);
            DrawSaveObjectEditor(demoSaveObject);
        }


        private void DrawSaveObjectEditor(SaveObject targetSaveObject)
        {
            if (!editorsLookup.ContainsKey(targetSaveObject))
            {
                editorsLookup.Add(targetSaveObject,
                    (SaveObjectEditor)UnityEditor.Editor.CreateEditor(targetSaveObject));
            }

            EditorGUILayout.BeginVertical(editorsLookup[targetSaveObject].serializedObject.Fp("isExpanded").boolValue
                ? "HelpBox"
                : "Box");

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            editorsLookup[targetSaveObject].serializedObject.Fp("isExpanded").boolValue =
                EditorGUILayout.Foldout(editorsLookup[targetSaveObject].serializedObject.Fp("isExpanded").boolValue,
                    targetSaveObject.name);

            if (EditorGUI.EndChangeCheck())
            {
                editorsLookup[targetSaveObject].serializedObject.ApplyModifiedProperties();
                editorsLookup[targetSaveObject].serializedObject.Update();

                SaveManager.Save();
            }

            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            GUI.backgroundColor = UtilEditor.Red;

            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Reset Save Object",
                        "Are you sure you want to reset all values on this save object?", "Reset", "Cancel"))
                {
                    // Reset Save Object
                    targetSaveObject.ResetObjectSaveValues();

                    editorsLookup[targetSaveObject].serializedObject.ApplyModifiedProperties();
                    editorsLookup[targetSaveObject].serializedObject.Update();

                    SaveManager.Save();
                    GUI.FocusControl(null);

                    return;
                }
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2.5f);


            if (editorsLookup[targetSaveObject].serializedObject.Fp("isExpanded").boolValue)
            {
                editorsLookup[targetSaveObject].EditorWindowGUI();
                GUILayout.Space(1.5f);
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();

            GUILayout.Space(4f);
        }



        public void RepaintAll()
        {
            foreach (var keyPair in editorsLookup)
            {
                keyPair.Value.serializedObject.Update();
            }
        }
        
        
        public void RefreshEditor()
        {
            foreach (var editor in editorsLookup.Values.ToArray())
            {
                editor.serializedObject.ApplyModifiedProperties();
                editor.serializedObject.Update();
            }
        }
    }
}