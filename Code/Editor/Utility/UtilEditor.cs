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

using System.Collections;
using System.Collections.Generic;
using CarterGames.Assets.SaveManager.Utility;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// The primary utility class for the save manager's editor logic. Should only be used in the editor space!
    /// </summary>
    public static class UtilEditor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        // Paths
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        public const string SettingsWindowPath = "Project/Carter Games/Save Manager";
        public static readonly string CapturesSavePath = $"{AssetBasePath}/Carter Games/Save Manager/Data/Save Profiles/";


        // Filters
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        // Graphics
        private const string SaveManagerLogoFilter = "SaveManagerLogo";
        private const string SaveManagerBannerFilter = "SaveManagerEditorHeader";
        private const string SaveManagerTitleBannerFilter = "SaveManagerTitleGraphic";
        private const string CarterGamesBannerFilter = "CarterGamesBanner";
        private const string CogIconFilter = "CogIcon";
        private const string KeyIconFilter = "KeyIcon";
        private const string BookIconFilter = "BookIcon";
        private const string DataIconFilter = "DataIcon";
        private const string SaveWhiteIconFilter = "SaveWhite";
        private const string EditorWindowTitleEditorFilter = "EditorTitle";
        private const string EditorWindowTitleProfilesFilter = "ProfilesTitle";


        // Texture Caches
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        private static Texture2D saveManagerLogoCache;
        private static Texture2D saveManagerBannerCache;
        private static Texture2D saveManagerTitleBannerCache;
        private static Texture2D carterGamesBannerCache;
        private static Texture2D cogIconCache;
        private static Texture2D keyIconCache;
        private static Texture2D bookIconCache;
        private static Texture2D dataIconCache;
        private static Texture2D saveIconWhite;
        private static Texture2D editorWindowTitleCreate;
        private static Texture2D editorWindowTitleEditor;
        private static Texture2D editorWindowTitleProfiles;


        // Asset Caches
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        private static SettingsAssetRuntime settingsCache;
        private static SerializedObject settingsObjectCache;
        private static SettingsAssetEditor settingsAssetEditorCache;
        private static SerializedObject editorSettingsObjectCache;
        private static SaveProfilesStore saveProfilesStoreCache;
        private static SaveData saveDataCache;
        private static EncryptionKeyAsset encryptionKeyAssetCache;
        private static AssetIndex assetIndexCache;


        // Colours
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Green Apple (Green) Color
        /// </summary>
        /// <see cref="https://www.computerhope.com/cgi-bin/htmlcolor.pl?c=4CC417"/>
        public static readonly Color Green = new Color32(76, 196, 23, 255);

        
        /// <summary>
        /// Rubber Ducky Yellow (Yellow) Color
        /// </summary>
        /// <see cref="https://www.computerhope.com/cgi-bin/htmlcolor.pl?c=FFD801"/>
        public static readonly Color Yellow = new Color32(255, 216, 1, 255);
        

        /// <summary>
        /// Scarlet Red (Red) Color
        /// </summary>
        /// <see cref="https://www.computerhope.com/cgi-bin/htmlcolor.pl?c=FF2400"/>
        public static readonly Color Red = new Color32(255, 36, 23, 255);


        // Misc
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        public static Texture[] SaveEditorButtonGraphics =
        {
            EditorWindowTitleEditor,
            EditorWindowTitleProfiles
        };


        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets the path where the asset code is located.
        /// </summary>
        private static string AssetBasePath => FileEditorUtil.AssetBasePath;


        // Textures/Graphics
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets the save manager logo.
        /// </summary>
        public static Texture2D SaveManagerLogo => FileEditorUtil.GetOrAssignCache(ref saveManagerLogoCache, SaveManagerLogoFilter);


        /// <summary>
        /// Gets the save manager banner.
        /// </summary>
        public static Texture2D SaveManagerBanner =>
            FileEditorUtil.GetOrAssignCache(ref saveManagerBannerCache, SaveManagerBannerFilter);


        /// <summary>
        /// Gets the save manager banner.
        /// </summary>
        public static Texture2D SaveManagerBannerTextOnly =>
            FileEditorUtil.GetOrAssignCache(ref saveManagerTitleBannerCache, SaveManagerTitleBannerFilter);


        /// <summary>
        /// Gets the carter games banner.
        /// </summary>
        public static Texture2D CarterGamesBanner =>
            FileEditorUtil.GetOrAssignCache(ref carterGamesBannerCache, CarterGamesBannerFilter);


        /// <summary>
        /// Gets the cog icon.
        /// </summary>
        public static Texture2D CogIcon => FileEditorUtil.GetOrAssignCache(ref cogIconCache, CogIconFilter);


        /// <summary>
        /// Gets the key icon.
        /// </summary>
        public static Texture2D KeyIcon => FileEditorUtil.GetOrAssignCache(ref keyIconCache, KeyIconFilter);


        /// <summary>
        /// Gets the book icon.
        /// </summary>
        public static Texture2D BookIcon => FileEditorUtil.GetOrAssignCache(ref bookIconCache, BookIconFilter);


        /// <summary>
        /// Gets the data icon.
        /// </summary>
        public static Texture2D DataIcon => FileEditorUtil.GetOrAssignCache(ref dataIconCache, DataIconFilter);


        /// <summary>
        /// Gets the white save icon.
        /// </summary>
        public static Texture2D SaveIconWhite => FileEditorUtil.GetOrAssignCache(ref saveIconWhite, SaveWhiteIconFilter);


        /// <summary>
        /// Gets the title text for the editor tab in the save manager editor window.
        /// </summary>
        public static Texture2D EditorWindowTitleEditor => 
            FileEditorUtil.GetOrAssignCache(ref editorWindowTitleEditor, EditorWindowTitleEditorFilter);


        /// <summary>
        /// Gets the title text for the profiles tab in the save manager editor window.
        /// </summary>
        public static Texture2D EditorWindowTitleProfiles =>
            FileEditorUtil.GetOrAssignCache(ref editorWindowTitleProfiles, EditorWindowTitleProfilesFilter);



        // Assets
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if there is a settings asset in the project.
        /// </summary>
        public static bool HasInitialized
        {
            get
            {
                AssetIndexHandler.UpdateIndex();
                return ScriptableRef.HasAllAssets;
            }
        }


        /// <summary>
        /// Gets/Sets the save manager settings asset.
        /// </summary>
        public static SettingsAssetRuntime Settings => ScriptableRef.RuntimeSettings;


        /// <summary>
        /// Gets/Sets the save manager editor settings asset.
        /// </summary>
        public static SerializedObject SettingsObject => ScriptableRef.RuntimeSettingsObject;


        /// <summary>
        /// Gets/Sets the save manager editor settings asset.
        /// </summary>
        public static SettingsAssetEditor SettingsAssetEditor => ScriptableRef.EditorSettings;


        /// <summary>
        /// Gets/Sets the save manager editor settings asset.
        /// </summary>
        public static SerializedObject EditorSettingsObject => ScriptableRef.EditorSettingsObject;



        /// <summary>
        /// Gets/Sets the save manager save profiles asset.
        /// </summary>
        public static SaveProfilesStore SaveProfiles => ScriptableRef.SaveProfiles;


        /// <summary>
        /// Gets/Sets the save manager save data asset.
        /// </summary>
        public static SaveData SaveData => ScriptableRef.SaveData;


        /// <summary>
        /// Gets/Sets the save manager save data asset.
        /// </summary>
        public static EncryptionKeyAsset EncryptionKeyAsset => ScriptableRef.EncryptionKey;
        
        
        /// <summary>
        /// Gets/Sets the save manager save data asset.
        /// </summary>
        public static AssetIndex AssetIndex => ScriptableRef.AssetIndex;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Draws the default Banner Logo header for the asset...
        /// </summary>
        public static void DrawHeader()
        {
            GUILayout.Space(5f);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (SaveManagerBanner != null)
            {
                if (GUILayout.Button(SaveManagerBanner, GUIStyle.none, GUILayout.MaxHeight(110)))
                {
                    GUI.FocusControl(null);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);
        }


        /// <summary>
        /// Draws the script fields in the custom inspector...
        /// </summary>
        public static void DrawMonoScriptSection<T>(T target) where T : MonoBehaviour
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour(target), typeof(T), false);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the script fields in the custom inspector...
        /// </summary>
        public static void DrawSoScriptSection(object target)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script:", MonoScript.FromScriptableObject((ScriptableObject)target),
                typeof(object), false);
            EditorGUI.EndDisabledGroup();
        }


        /// <summary>
        /// Draws a horizontal line
        /// </summary>
        public static void DrawHorizontalGUILine()
        {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = new Texture2D(1, 1);
            boxStyle.normal.background.SetPixel(0, 0, Color.grey);
            boxStyle.normal.background.Apply();

            var one = EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", boxStyle, GUILayout.ExpandWidth(true), GUILayout.Height(2));
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Creates a deselect zone to let users click outside of any editor window to unfocus from their last selected field.
        /// </summary>
        /// <param name="rect">The rect to draw on.</param>
        public static void CreateDeselectZone(ref Rect rect)
        {
            if (rect.width <= 0)
            {
                rect = new Rect(0, 0, Screen.width, Screen.height);
            }

            if (GUI.Button(rect, string.Empty, GUIStyle.none))
            {
                GUI.FocusControl(null);
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Helper Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public static void Initialize()
        {
            AssetDatabase.Refresh();
            
            if (assetIndexCache == null)
            {
                assetIndexCache = AssetIndex;
            }
            
            if (settingsCache == null)
            {
                settingsCache = Settings;
            }

            if (saveDataCache == null)
            {
                saveDataCache = SaveData;
            }

            if (settingsAssetEditorCache == null)
            {
                settingsAssetEditorCache = SettingsAssetEditor;
            }

            if (saveProfilesStoreCache == null)
            {
                saveProfilesStoreCache = SaveProfiles;
            }

            if (encryptionKeyAssetCache == null)
            {
                encryptionKeyAssetCache = EncryptionKeyAsset;
            }

            AssetIndexHandler.UpdateIndex();
            EditorUtility.SetDirty(AssetIndex);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            

            var dataObj = new SerializedObject(Settings);
            dataObj.FindProperty("saveDataAsset").objectReferenceValue = SaveData;
            dataObj.ApplyModifiedProperties();
            dataObj.Update();

            dataObj = new SerializedObject(SaveData);
            
            if (dataObj.FindProperty("saveData").arraySize <= 0)
            {
                dataObj.FindProperty("saveData").ClearArray();

                var objs = GetAllInstances<SaveObject>();

                for (var i = 0; i < objs.Length; i++)
                {
                    dataObj.FindProperty("saveData").InsertArrayElementAtIndex(i);
                    dataObj.FindProperty("saveData").GetArrayElementAtIndex(i).objectReferenceValue = objs[i];
                }
            }

            dataObj.ApplyModifiedProperties();
            dataObj.Update();

            Settings.Initialize();
            SettingsAssetEditor.Initialize();
            
            if (!EncryptionKeyAsset.HasKey)
            {
                EncryptionKeyAsset.SaveEncryptionKey = EncryptionKeyAsset.GenerateKey();
                EditorUtility.SetDirty(EncryptionKeyAsset);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        
        public static T[] GetAllInstances<T>() where T : SaveManagerAsset
        {
            var guids = AssetDatabase.FindAssets("t:"+ typeof(T).Name);
            var a = new T[guids.Length];
            
            for (var i =0;i<guids.Length;i++)         //probably could get optimized 
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
 
            return a;
        }


        public static void ForceUpdateSaveDataAsset()
        {
            var dataObj = new SerializedObject(SaveData);
            dataObj.FindProperty("saveData").ClearArray();

            var objects = GetAllInstances<SaveObject>();
            
            for (var i = 0; i < objects.Length; i++)
            {
                dataObj.FindProperty("saveData").InsertArrayElementAtIndex(i);
                dataObj.FindProperty("saveData").GetArrayElementAtIndex(i).objectReferenceValue = objects[i];
            }
            
            dataObj.ApplyModifiedProperties();
            dataObj.Update();
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        /// <summary>
        /// Checks to see if a list has any null entries.
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <returns>If there was a null entry or not.</returns>
        public static bool HasNullEntries(this IList list)
        {
            if (list == null || list.Count <= 0) return false;

            for (var i = list.Count - 1; i > -1; i--)
            {
                if (list[i] != null) continue;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Removes missing entries in a list when called.
        /// </summary>
        /// <param name="list">The list to edit.</param>
        /// <returns>The list with no null entries.</returns>
        public static IList RemoveMissing(this IList list)
        {
            for (var i = list.Count - 1; i > -1; i--)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                }
            }

            return list;
        }


        /// <summary>
        /// Removed any duplicate entries from the list entered.
        /// </summary>
        /// <param name="list">The list to edit.</param>
        /// <typeparam name="T">The type for the list.</typeparam>
        /// <returns>The list with no null entries.</returns>
        public static IList RemoveDuplicates<T>(this IList list)
        {
            var validateList = new List<T>();

            for (var i = list.Count - 1; i > -1; i--)
            {
                if (validateList.Contains((T)list[i])) continue;
                validateList.Add((T)list[i]);
            }

            return validateList;
        }


        /// <summary>
        /// Refreshes the image cache list if an entry is null.
        /// </summary>
        public static void TryRefreshImageCache()
        {
            if (SaveEditorButtonGraphics == null || SaveEditorButtonGraphics[0] == null)
            {
                SaveEditorButtonGraphics = new Texture[2] { EditorWindowTitleEditor, EditorWindowTitleProfiles };
            }
        }


        [MenuItem("Tools/Carter Games/Save Manager/Reset Save Player Prefs", priority = 18)]
        private static void ResetPrefs()
        {
            if (EditorUtility.DisplayDialog("Reset Asset Player Prefs",
                    "Resetting the prefs can cause issues with existing saves of your game. Are you sure you want to do this?",
                    "Yes", "Cancel"))
            {
                UtilRuntime.SetPref(PrefSetting.Encryption, PrefUseOption.Current, 0);
                UtilRuntime.SetPref(PrefSetting.Encryption, PrefUseOption.Build, 0);
                UtilRuntime.SetPref(PrefSetting.Encryption, PrefUseOption.Editor, 0);
                UtilRuntime.SetPref(PrefSetting.Encryption, PrefUseOption.EditorCurrent, 0);
            }
        }
    }
}