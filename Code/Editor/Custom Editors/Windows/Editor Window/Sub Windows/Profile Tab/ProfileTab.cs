using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor.SubWindows
{
    /// <summary>
    /// Draws the profile tab of the save editor.
    /// </summary>
    public sealed class ProfileTab
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        // Help box dialogue.
        private const string NoSaveObjectsDialogue =
            "You don't have any save objects setup, so this tool is disabled. Create & add a save object implementation to use the save profiles tab.";
        
        // Fields
        private static Rect deselectRect;
        private static Vector2 scrollRect;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public void DrawTab(List<SaveObject> saveObjects)
        {
            if (saveObjects.Count <= 0)
            {
                // No save objects, so disable.
                EditorGUILayout.HelpBox(NoSaveObjectsDialogue, MessageType.Info);
            }
            else
            {
                scrollRect = EditorGUILayout.BeginScrollView(scrollRect);
                
                EditorGUILayout.BeginVertical("HelpBox");
                SaveProfileCreatorGUI.DrawDisplay();
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.BeginVertical("HelpBox");
                SaveProfileListGUI.DrawDisplay();
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.Space(10);
                EditorGUILayout.EndScrollView();
            }
            
            
            // Draws a deselect box around the rest of the GUI.
            UtilEditor.CreateDeselectZone(ref deselectRect);
        }
    }
}