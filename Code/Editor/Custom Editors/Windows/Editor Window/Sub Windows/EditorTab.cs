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
        
        private Dictionary<SaveObject, UnityEditor.Editor> editorsLookup;
        private Dictionary<SaveObject, SerializedObject> soLookup;
        private Dictionary<SaveObject, SerializedObject> demoLookup;
        
        private static Rect deselectRect;
        private static Vector2 scrollRect;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Initialize Method
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public void Initialize()
        {
            soLookup = new Dictionary<SaveObject, SerializedObject>();
            editorsLookup = new Dictionary<SaveObject, UnityEditor.Editor>();
            demoLookup = new Dictionary<SaveObject, SerializedObject>();
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

            scrollRect = EditorGUILayout.BeginScrollView(scrollRect);

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
                

                if (saveObj.GetType().FullName == "CarterGames.Assets.SaveManager.Demo.ExampleSaveObject")
                {
                    if (demoLookup.ContainsKey(saveObj)) continue;
                    demoLookup.Add(saveObj, sObj);
                }
                else
                {
                    if (soLookup.ContainsKey(saveObj)) continue;
                    soLookup.Add(saveObj, sObj);
                }
            }


            if (soLookup.Count > 0)
            {
                DrawSaveObjects(" Save Objects", ref soLookup);
            }
                
            
            if (demoLookup.Count > 0)
            {
                GUILayout.Space(12.5f);
                DrawSaveObjects(" Demo Save Objects", ref demoLookup);
            }
            
            EditorGUILayout.Space(5f);
            EditorGUILayout.EndScrollView();
            
            UtilEditor.CreateDeselectZone(ref deselectRect);
        }


        /// <summary>
        /// Draws a collection of save objects.
        /// </summary>
        /// <param name="sectionName">The name for the section.</param>
        /// <param name="lookup">The lookup for the objects.</param>
        private void DrawSaveObjects(string sectionName, ref Dictionary<SaveObject, SerializedObject> lookup)
        {
            EditorGUILayout.LabelField(sectionName, EditorStyles.boldLabel);
            
            
            foreach (var pair in lookup)
            {
                if (!editorsLookup.ContainsKey(pair.Key))
                {
                    editorsLookup.Add(pair.Key, UnityEditor.Editor.CreateEditor(pair.Key));
                }
                
                
                EditorGUILayout.BeginVertical("HelpBox");
                
                EditorGUILayout.BeginHorizontal();
                
                EditorGUI.BeginChangeCheck();

                editorsLookup[pair.Key].serializedObject.FindProperty("isExpanded").boolValue =
                    EditorGUILayout.Foldout(editorsLookup[pair.Key].serializedObject.FindProperty("isExpanded").boolValue, pair.Key.name);
                
                if (EditorGUI.EndChangeCheck())
                {
                    editorsLookup[pair.Key].serializedObject.ApplyModifiedProperties();
                    editorsLookup[pair.Key].serializedObject.Update();
                    
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
                        pair.Key.ResetObjectSaveValues();
                        
                        editorsLookup[pair.Key].serializedObject.ApplyModifiedProperties();
                        editorsLookup[pair.Key].serializedObject.Update();
                        
                        SaveManager.Save();
                        GUI.FocusControl(null);

                        return;
                    }
                }

                GUI.backgroundColor = UtilEditor.SettingsAssetEditor.BackgroundColor;
                
                EditorGUILayout.EndHorizontal();
                
                GUILayout.Space(2.5f);

                
                if (editorsLookup[pair.Key].serializedObject.FindProperty("isExpanded").boolValue)
                {
                    editorsLookup[pair.Key].OnInspectorGUI();
                    GUILayout.Space(1.5f);
                }
                
                EditorGUI.EndDisabledGroup();
                
                EditorGUILayout.EndVertical();
            }
        }


        public void RefreshEditor()
        {
            foreach (var editor in editorsLookup.Values.ToArray())
            {
                editor.serializedObject.Update();
                editor.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}