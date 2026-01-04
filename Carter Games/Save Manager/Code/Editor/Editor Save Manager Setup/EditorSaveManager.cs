/*
 * Save Manager (3.x)
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

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles a few editor setup bits for the save manager setup to keep the save up-to date.
    /// </summary>
    public sealed class EditorSaveManager : UnityEditor.AssetModificationProcessor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Get/Set if the editor is dirty (editor perf).
        /// </summary>
        private static bool SaveManagerEditorIsDirty
        {
            get => (bool) PerUserSettingsEditor.GetOrCreateValue<bool>("save_manager_dirty", PerUserSettingType.EditorPref);
            set => PerUserSettingsEditor.SetValue<bool>("save_manager_dirty", PerUserSettingType.EditorPref, value);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Auto-runs on compile.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void InitializeEditorSaveManager()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnPreAssemblyCompile;
            AssemblyReloadEvents.beforeAssemblyReload += OnPreAssemblyCompile;

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            
            EditorApplication.quitting -= OnEditorQuitting;
            EditorApplication.quitting += OnEditorQuitting;
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Ensures the game saves when a script re-compile is about to happen in the editor.
        /// </summary>
        private static void OnPreAssemblyCompile()
        {
            if (!SaveManagerEditorIsDirty)
            {
                SmDebugLogger.LogDev("Editor Save [Script Recompile]: Will not save as no changes detected.");
                return;
            }

            SmDebugLogger.LogDev("Editor Save [Script Recompile]: Saving.");
            EditorSaveGameData();
        }


        /// <summary>
        /// Ensures the game saves when the editor is about to be quitted from.
        /// </summary>
        private static void OnEditorQuitting()
        {
            if (!SaveManagerEditorIsDirty)
            {
                SmDebugLogger.LogDev("Editor Save [Application Close]: Will not save as no changes detected.");
                return;
            }
            
            SmDebugLogger.LogDev("Editor Save [Application Close]: Saving.");
            EditorSaveGameData();
        }


        /// <summary>
        /// Ensures the game saves when the editor is exiting edit mode.
        /// </summary>
        /// <param name="change">The state change that occurred.</param>
        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            // Only trigger on enter play mode.
            if (change != PlayModeStateChange.ExitingEditMode) return;
            
            if (!SaveManagerEditorIsDirty)
            {
                SmDebugLogger.LogDev("Editor Save [Entering Play Mode]: Will not save as no changes detected.");
                return;
            }
            
            SmDebugLogger.LogDev("Editor Save [Entering Play Mode]: Saving.");
            EditorSaveGameData();
        }
        
        
        /// <summary>
        /// Ensures the game saves when the editor is saving assets.
        /// </summary>
        /// <param name="paths">Not used</param>
        /// <returns>String[] (Not used)</returns>
        private static string[] OnWillSaveAssets(string[] paths)
        {
            if (!SaveManagerEditorIsDirty)
            {
                SmDebugLogger.LogDev("Editor Save [Save Project]: Will not save as no changes detected.");
                return paths;
            }
            
            SmDebugLogger.LogDev("Editor Save [Save Project]: Saving.");
            EditorSaveGameData();
            return paths;
        }


        /// <summary>
        /// Triggers the editor to save when called.
        /// </summary>
        private static void EditorSaveGameData()
        {
            SaveManager.SaveGame();
            SaveManagerEditorIsDirty = false; 
        }
        

        /// <summary>
        /// Tries to set the editor save state as dirty (so it registers as changes made).
        /// </summary>
        public static void TrySetDirty()
        {
            if (SaveManagerEditorIsDirty) return;
            SaveManagerEditorIsDirty = true;
        }
    }
}