using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
    [InitializeOnLoad]
    public static class EditorSaveHandler
    {
        /// <summary>
        /// Get/Set if the editor is dirty (editor perf).
        /// </summary>
        private static bool SaveManagerEditorIsDirty
        {
            get => (bool) PerUserSettingsEditor.GetOrCreateValue<bool>("save_manager_dirty", PerUserSettingType.EditorPref);
            set => PerUserSettingsEditor.SetValue<bool>("save_manager_dirty", PerUserSettingType.EditorPref, value);
        }
        
        
        static EditorSaveHandler()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnPreAssemblyCompile;
            AssemblyReloadEvents.beforeAssemblyReload += OnPreAssemblyCompile;

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            
            EditorApplication.quitting -= OnEditorQuitting;
            EditorApplication.quitting += OnEditorQuitting;
            
            SaveManagerInitializer.InitializeSaveManagerRuntimeSetup();
            EditorSaveObjectController.ReInitIfNeeded();
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
            if (change == PlayModeStateChange.ExitingPlayMode)
            {
                EditorSaveObjectController.ReInitIfNeeded();
                return;
            }
            
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