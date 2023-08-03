using System.IO;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Draws the create GUI in the profiles tab of the save editor.
    /// </summary>
    public static class SaveProfileCreatorGUI
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        // General text.
        private const string DropDownLabel = "Profile Creator";
        private const string HelpBoxText = "Use this tool to make captures of the current save data that you can load back to. Note changing the save value structure of captured data will reset its data in the captures.";
        private const string CreateProfileButtonLabel = "Create Profile";
        
        // User fields GUI content
        private static readonly GUIContent SavePathContent = new GUIContent("Will be saved to:", "The location where the profiles are saved to.");
        private static readonly GUIContent ProfileNameContent = new GUIContent("Profile name:", "Set the name for the profile.");

        // Override dialogue text.
        private const string OverrideTitle = "Profile Creation Warning";
        private const string OverrideBody = "A profile of this name already exists! Do you want to override it?";
        private const string OverrideYes = "Override";
        private const string OverrideNo = "Cancel";

        // User fields
        private static string profileName = UtilEditor.SettingsAssetEditor.LastSaveProfileName;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Draws the display to let the user create save profiles.
        /// </summary>
        public static void DrawDisplay()
        {
            // Draws the dropdown for this GUI.
            UtilEditor.SettingsAssetEditor.ProfileCreatorExpanded =
                EditorGUILayout.Foldout(UtilEditor.SettingsAssetEditor.ProfileCreatorExpanded, DropDownLabel);

            
            // Stop if the dropdown is not opened.
            if (!UtilEditor.SettingsAssetEditor.ProfileCreatorExpanded) return;

            
            // Draws the help box section.
            EditorGUILayout.Space(7.5f);
            EditorGUILayout.HelpBox(HelpBoxText, MessageType.Info);
            EditorGUILayout.Space(7.5f);

            
            // Draw profile options for the user to set.
            DrawProfileOptions();
                
            
            // Draws a little extra space before the create button.
            EditorGUILayout.Space(2.5f);    
            
            
            // Disables the create button if there is nothing entered into the profile name field.
            if (profileName == null)
            {
                profileName = string.Empty;
            }
            
            EditorGUI.BeginDisabledGroup(profileName.Length <= 0 || UtilEditor.CapturesSavePath.Length <= 0);
            
            
            // Draws the create profile button
            DrawCreateButton();
            
            
            EditorGUI.EndDisabledGroup();

            // Draws a little extra space under the create button.
            EditorGUILayout.Space(2.5f);
        }


        /// <summary>
        /// Draws the profile name & location fields.
        /// </summary>
        private static void DrawProfileOptions()
        {
            // Location Section
            EditorGUILayout.BeginHorizontal();
            
            
            EditorGUILayout.LabelField(SavePathContent, GUILayout.Width(100f));
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(UtilEditor.CapturesSavePath + profileName);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
            
            
            // Profile Name Section
            EditorGUILayout.BeginHorizontal();

            
            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            
            EditorGUILayout.LabelField(ProfileNameContent, GUILayout.Width(100f));
            profileName = EditorGUILayout.TextField(GUIContent.none, profileName);

            if (EditorGUI.EndChangeCheck())
            {
                UpdateSavedProfileName(profileName);
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Draws the create profile button.
        /// </summary>
        private static void DrawCreateButton()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            
            // Draws the create profile button
            GUI.backgroundColor = UtilEditor.Green;
            
            if (GUILayout.Button(CreateProfileButtonLabel, GUILayout.Width(125f)))
            {
                OnCreateProfilePressed();
            }

            GUI.backgroundColor = UtilEditor.SettingsAssetEditor.BackgroundColor;
            
            EditorGUI.EndDisabledGroup();
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Handles the logic to create a profile based on the current setup.
        /// </summary>
        private static void OnCreateProfilePressed()
        {
            if (Directory.Exists(UtilEditor.CapturesSavePath + profileName))
            {
                if (!EditorUtility.DisplayDialog(OverrideTitle, OverrideBody, OverrideYes, OverrideNo))
                {
                    return;
                }

                FileEditorUtil.DeleteDirectoryAndContents(UtilEditor.CapturesSavePath + profileName);
            }
                    
            SaveProfileManager.CaptureProfile(UtilEditor.CapturesSavePath, profileName);
            UpdateSavedProfileName(string.Empty);
            profileName = string.Empty;
            GUI.FocusControl(null);
        }


        /// <summary>
        /// Updates the saved profile name in the editor settings asset to the value entered when called.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        private static void UpdateSavedProfileName(string value)
        {
            UtilEditor.SettingsAssetEditor.LastSaveProfileName = value;
            EditorUtility.SetDirty(UtilEditor.SettingsAssetEditor);
            AssetDatabase.SaveAssets();
        }
    }
}