using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class SaveEditorGlobalTab
    {
        private Dictionary<string, IEnumerable<SaveObject>> categoriesLookup;


        private Vector2 ScrollPos
        {
            get => EditorUserSettings.GetVec2("cg_sm_global_scroll_pos");
            set => EditorUserSettings.SetVec2("cg_sm_global_scroll_pos", value);
        }
        
        
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

            // if (EditorGUI.EndChangeCheck())
            // {
            //     SaveManager.SaveGame();
            // }
        }
    }
}