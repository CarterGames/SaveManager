﻿/*
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
using System.Linq;
using CarterGames.Assets.SaveManager.Editor.SubWindows;
using CarterGames.Common;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public sealed class SaveManagerEditorWindow : EditorWindow
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static bool isInitialized;
        
        private List<SaveObject> saveObjects;
        
        private EditorTab editorTab;
        private ProfileTab profilesTab;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises when a new save object is added to the data.
        /// </summary>
        public static readonly Evt SaveObjectAddedToSaveData = new Evt();
        
        
        public static readonly Evt RequestRepaint = new Evt();

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Window Access Method
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [MenuItem("Tools/Carter Games/Save Manager/Save Editor", priority = 15)]
        private static void ShowWindow()
        {
            isInitialized = false;
            
            var window = GetWindow<SaveManagerEditorWindow>();
            window.titleContent = new GUIContent("Save Editor", UtilEditor.SaveIconWhite);
            window.Show();
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnEnable()
        {
            isInitialized = false;
        }
        

        private void OnGUI()
        {
            Initialize();
            
            RequestRepaint.Add(Repaint);
            
            EditorGUILayout.Space(17.5f);

            EditorGUI.BeginChangeCheck();

            if (UtilEditor.SaveEditorButtonGraphics.Any(t => t == null))
            {
                PerUserSettings.SaveEditorTabPos =
                    GUILayout.Toolbar(PerUserSettings.SaveEditorTabPos, new string[2] { "Editor", "Profiles" }, GUILayout.Height(30f));
            }
            else
            {
                PerUserSettings.SaveEditorTabPos =
                    GUILayout.Toolbar(PerUserSettings.SaveEditorTabPos, UtilEditor.SaveEditorButtonGraphics, GUILayout.Height(30f));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (PerUserSettings.SaveEditorTabPos.Equals(0))
                {
                    editorTab.RefreshEditor();
                }
                
                AssetDatabase.SaveAssets();
            }
            
            EditorGUILayout.Space(7.5f);

            switch (PerUserSettings.SaveEditorTabPos)
            {
                case 0:
                    editorTab.DrawTab(saveObjects);
                    break;
                case 1:
                    profilesTab.DrawTab(saveObjects);
                    break;
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Utility Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initializes the window when called if its not already.
        /// </summary>
        private void Initialize()
        {
            if (isInitialized) return;

            UtilEditor.ForceUpdateSaveDataAsset();
            AssetIndexHandler.UpdateIndex();
            
            editorTab = new EditorTab();
            profilesTab = new ProfileTab();

            editorTab.Initialize();
            
            SetSaveObjectsCache();
            
            SaveObjectAddedToSaveData.Remove(SetSaveObjectsCache);
            SaveObjectAddedToSaveData.Add(SetSaveObjectsCache);
            
            UtilEditor.TryRefreshImageCache();
            
            EditorApplication.delayCall -= editorTab.RepaintAll;
            EditorApplication.delayCall += editorTab.RepaintAll;

            EditorApplication.playModeStateChanged -= OnPlayStateChanged;
            EditorApplication.playModeStateChanged += OnPlayStateChanged;
            
            isInitialized = true;
        }


        /// <summary>
        /// Sets the save object cache, used to also refresh it when needed.
        /// </summary>
        private void SetSaveObjectsCache()
        {
            saveObjects = new List<SaveObject>();
            saveObjects = UtilEditor.SaveData.Data;

            if (!saveObjects.HasNullEntries()) return;
            
            saveObjects = (List<SaveObject>) saveObjects.RemoveMissing().RemoveDuplicates<SaveObject>();
            UtilEditor.SaveData.Data = saveObjects;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        private void OnPlayStateChanged(PlayModeStateChange stateChange)
        {
            if (stateChange != PlayModeStateChange.ExitingPlayMode) return;
            editorTab.RepaintAll();
        }
    }
}