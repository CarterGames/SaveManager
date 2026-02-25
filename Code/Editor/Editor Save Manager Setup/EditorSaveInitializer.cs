using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;

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
            
            EditorSaveObjectController.InitializedEditorEvt.Add(OnSaveObjectInit);
            EditorSaveObjectController.Initialize();
            return;

            void OnSaveObjectInit()
            {
                if (ScriptableRef.GetAssetDef<DataAssetSettings>().AssetRef.Location.HasSaveData) return;
                SaveManager.SaveGame();
                EditorApplication.update -= OnEditorUpdate;
            }
        }
    }
}