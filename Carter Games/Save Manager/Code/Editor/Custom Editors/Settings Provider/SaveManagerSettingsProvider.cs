/*
 * Copyright (c) 2018-Present Carter Games
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
using System.IO;
using CarterGames.Assets.SaveManager.Utility;
using UnityEditor;
using UnityEngine;
using SettingsProvider = UnityEditor.SettingsProvider;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class SaveManagerSettingsProvider
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        // Text
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static readonly GUIContent VersionTitle = new GUIContent("Version", "The version of the asset in use.");
        private static readonly GUIContent VersionValue = new GUIContent(AssetVersionData.VersionNumber, "The version you currently have installed.");
        private static readonly GUIContent ReleaseTitle = new GUIContent("Release Date", "The date this version of the asset was published on.");
        private static readonly GUIContent ReleaseValue = new GUIContent(AssetVersionData.ReleaseDate, "The version you currently have installed.");
        
        private static readonly GUIContent EncryptionOption = new GUIContent("Encryption Option", "Sets which encryption setting the save will use.");
        private static readonly GUIContent PrettyFormat = new GUIContent("Pretty Save Formatting?","Formats the save file into a more readable format when saving.");
        private static readonly GUIContent AutoSave = new GUIContent("Auto Save?","Defines if the game saves when exiting the game.");
        private static readonly GUIContent SaveKeysToggle = new GUIContent("Show Save Keys?","Defines if the save editor shows the save key for each save value, Use this to condense the UI a tad if you need to.");
        private static readonly GUIContent Logs = new GUIContent("Show Log Messages?", "Shows log messages for any errors as well as some handy debugging information.");

        private const string ExplorerButtonLabel = "Open Save Path In Explorer";
        private const string ExplorerButtonLabelWeb = "Open Save Path In Explorer (Editor Only)";
        
        
        // References
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static SettingsProvider provider;
        private static SerializedObject settingsAssetObject;
        private static Rect deselectRect;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the settings asset in the project as a SerializedObject.
        /// </summary>
        private static SerializedObject SettingsAssetObject
        {
            get
            {
                if (settingsAssetObject != null) return settingsAssetObject;

                if (!UtilEditor.AssetIndex.Lookup.ContainsKey(typeof(SettingsAssetRuntime).ToString()))
                {
                    UtilEditor.Initialize();
                }
                
                settingsAssetObject = new SerializedObject(UtilEditor.AssetIndex.Lookup[typeof(SettingsAssetRuntime).ToString()][0]);
                return settingsAssetObject;
            }
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Menu Items
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The menu item for opening the settings window.
        /// </summary>
        [MenuItem("Tools/Carter Games/Save Manager/Edit Settings", priority = 0)]
        public static void OpenSettings()
        {
            SettingsService.OpenProjectSettings("Project/Carter Games/Save Manager");
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Handles the settings window in the engine.
        /// </summary>
        [SettingsProvider]
        public static SettingsProvider MultiSceneSettingsDrawer()
        {
            var provider = new SettingsProvider(UtilEditor.SettingsWindowPath, SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    UtilEditor.DrawHeader();
                    DrawInfo();
                    
                    EditorGUILayout.BeginVertical("HelpBox");
                    GUILayout.Space(1.5f);
            
                    EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                    GUILayout.Space(1.5f);
                    
                    EditorGUI.BeginChangeCheck();
                    
                    DrawGeneralOptions();

                    if (EditorGUI.EndChangeCheck())
                    {
                        SettingsAssetObject.ApplyModifiedProperties();
                        SettingsAssetObject.Update();
                    }

                    GUILayout.Space(2.5f);
                    EditorGUILayout.EndVertical();
                    
                    DrawButtons();
                    
                    UtilEditor.CreateDeselectZone(ref deselectRect);
                },
                
                keywords = new HashSet<string>(new[]
                {
                    "Carter Games", "External Assets", "Tools", "Save Manager", "Save", "Game Save Management"
                })
            };
            
            return provider;
        }


        /// <summary>
        /// Draws the info section of the window.
        /// </summary>
        private static void DrawInfo()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(VersionTitle,  VersionValue);
            VersionEditorGUI.DrawCheckForUpdatesButton();
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.LabelField(ReleaseTitle, ReleaseValue);

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the general options section of the window.
        /// </summary>
        private static void DrawGeneralOptions()
        {
            GUILayout.Space(3f);
            
            // Save Path...
#if UNITY_WEBGL && UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_STANDALONE_WIN
            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = UtilEditor.Yellow;
            
            if (GUILayout.Button(ExplorerButtonLabelWeb))
            {
                if (!Directory.Exists(UtilEditor.Settings.SavePath))
                {
                    FileEditorUtil.CreateToDirectory(UtilEditor.Settings.SavePath);
                }
                
                EditorUtility.RevealInFinder(UtilEditor.Settings.SavePath.Replace("save.sf", string.Empty));
            }
            
            GUI.backgroundColor = UtilEditor.SettingsAssetEditor.BackgroundColor;
            
            EditorGUILayout.EndHorizontal();
#else
            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = UtilEditor.Yellow;
            
            if (GUILayout.Button(ExplorerButtonLabel))
            {
                if (!Directory.Exists(UtilEditor.Settings.SavePath))
                {
                    FileEditorUtil.CreateToDirectory(UtilEditor.Settings.SavePath);
                }
                
                EditorUtility.RevealInFinder(UtilEditor.Settings.SavePath.Replace("save.sf", string.Empty));
            }
            
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndHorizontal();
#endif
            
            GUILayout.Space(3f);
            
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            PerUserSettings.EditorSettingsEditorTab = EditorGUILayout.Foldout(PerUserSettings.EditorSettingsEditorTab, "Editor");

            if (PerUserSettings.EditorSettingsEditorTab)
            {
                EditorGUILayout.BeginVertical("Box");
                
                PerUserSettings.VersionValidationAutoCheckOnLoad = EditorGUILayout.Toggle(new GUIContent("Update Check On Load",
                        "Checks for any updates to the asset from the GitHub page when you load the project."),
                    PerUserSettings.VersionValidationAutoCheckOnLoad);
                
                // Show Logs...
                PerUserSettingsRuntime.ShowDebugLogs = EditorGUILayout.Toggle(Logs, PerUserSettingsRuntime.ShowDebugLogs);
                
                // Editor Only Setting....
                PerUserSettings.ShowSaveKeys = EditorGUILayout.Toggle(SaveKeysToggle, PerUserSettings.ShowSaveKeys);
                
                // Use save categories...
                EditorGUI.BeginChangeCheck();
                
                PerUserSettings.SaveEditorOrganiseByCategory = EditorGUILayout.Toggle(new GUIContent("Show Save Categories",
                        "Organises the save editor by category attribute [SaveCategory()]."),
                    PerUserSettings.SaveEditorOrganiseByCategory);

                if (EditorGUI.EndChangeCheck())
                {
                    SaveManagerEditorWindow.RequestRepaint.Raise();
                }
                
                if (GUILayout.Button("Reset Settings"))
                {
                    if (EditorUtility.DisplayDialog("Reset Save Manager Settings",
                            "Are you sure that you want to reset all settings for the asset to their defaults. This only applies to you. Other users of the project will have to do this for their settings themselves.",
                            "Reset Settings", "Cancel"))
                    {
                        PerUserSettings.ResetPrefs();
                    }
                }
                
                EditorGUILayout.EndVertical();
            }
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
            
            GUILayout.Space(3f);
            
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            PerUserSettings.EditorSettingsSaveOptionsTab = EditorGUILayout.Foldout(PerUserSettings.EditorSettingsSaveOptionsTab, "Save Options");

            if (PerUserSettings.EditorSettingsSaveOptionsTab)
            {
                EditorGUILayout.BeginVertical("Box");
                
                // Encryption...
                EditorGUI.BeginChangeCheck();
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(SettingsAssetObject.Fp("encryptionOption"),EncryptionOption);
            
                if (EditorGUI.EndChangeCheck())
                {
                    UtilRuntime.SetPref(PrefSetting.Encryption, PrefUseOption.Editor,
                        SettingsAssetObject.Fp("encryptionOption").intValue);
                }

                // Pretty Format...
                EditorGUI.BeginDisabledGroup(SettingsAssetObject.Fp("encryptionOption").enumValueIndex.Equals(1));
                EditorGUILayout.PropertyField(SettingsAssetObject.Fp("prettify"),PrettyFormat);
                EditorGUI.EndDisabledGroup();
                
                EditorGUILayout.PropertyField(SettingsAssetObject.Fp("autoSaveOnExit"),AutoSave);
                
                EditorGUILayout.EndVertical();
            }
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the buttons section of the window.
        /// </summary>
        private static void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Buy Me A Coffee", GUILayout.Height(30), GUILayout.MinWidth(100)))
            {
                Application.OpenURL("https://www.buymeacoffee.com/cartergames");
            }
            
            if (GUILayout.Button("GitHub", GUILayout.Height(30), GUILayout.MinWidth(100)))
            {
                Application.OpenURL("https://github.com/CarterGames/SaveManager");
            }

            if (GUILayout.Button("Documentation", GUILayout.Height(30), GUILayout.MinWidth(100)))
            {
                Application.OpenURL("https://carter.games/savemanager");
            }

            if (GUILayout.Button("Support", GUILayout.Height(30), GUILayout.MinWidth(100)))
            {
                Application.OpenURL("https://carter.games/contact");
            }

            EditorGUILayout.EndHorizontal();
            
            
            
            if (UtilEditor.CarterGamesBanner != null)
            {
                var defaultTextColour = GUI.contentColor;
                GUI.contentColor = new Color(1, 1, 1, .75f);

                if (GUILayout.Button(UtilEditor.CarterGamesBanner, GUILayout.MaxHeight(40)))
                {
                    Application.OpenURL("https://carter.games");
                }

                GUI.contentColor = defaultTextColour;
            }
            else
            {
                if (GUILayout.Button("Carter Games", GUILayout.MaxHeight(40)))
                {
                    Application.OpenURL("https://carter.games");
                }
            }
        }
    }
}