using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
    [InitializeOnLoad]
    public static class EditorSaveManager
    {
        static EditorSaveManager()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnPreAssemblyCompile;
            AssemblyReloadEvents.beforeAssemblyReload += OnPreAssemblyCompile;

            EditorApplication.quitting -= OnEditorQuitting;
            EditorApplication.quitting += OnEditorQuitting;
        }

        
        private static void OnPreAssemblyCompile()
        {
            SmDebugLogger.LogDev("Editor saving due to script recompile about to happen.");
            SaveManager.SaveGame();
        }


        private static void OnEditorQuitting()
        {
            SmDebugLogger.LogDev("Editor saving due to application close.");
            SaveManager.SaveGame();
        }
    }
}