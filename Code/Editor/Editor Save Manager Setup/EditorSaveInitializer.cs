using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class EditorSaveInitializer
    {
        private static bool IsInitializing { get; set; }
        
        
        static EditorSaveInitializer()
        {
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
        }


        private static void OnEditorUpdate()
        {
            if (IsInitializing) return;
            IsInitializing = true;

            if (EditorSaveObjectController.IsInitialized)
            {
                EditorApplication.update -= OnEditorUpdate;
                return;
            }
            
            EditorSaveObjectController.InitializedEditorEvt.Add(OnSaveObjectInit);
            EditorSaveObjectController.Initialize();
            return;

            void OnSaveObjectInit()
            {
                SaveManager.SaveGame();
                EditorApplication.update -= OnEditorUpdate;
                Debug.LogError("Save init from import...");
            }
        }
    }
}