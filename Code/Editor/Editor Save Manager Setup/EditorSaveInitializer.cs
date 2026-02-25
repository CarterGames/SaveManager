using System;
using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class EditorSaveInitializer
    {
        private static bool IsInitializing { get; set; }


        public static void OnEditorUpdate(Action onComplete)
        {
            if (IsInitializing) return;
            IsInitializing = true;

            if (EditorSaveObjectController.IsInitialized)
            {
                onComplete?.Invoke();
                return;
            }
            
            EditorSaveObjectController.InitializedEditorEvt.Add(OnSaveObjectInit);
            EditorSaveObjectController.Initialize();
            Debug.LogError("Save init from import...1");
            return;

            void OnSaveObjectInit()
            {
                if (ScriptableRef.GetAssetDef<DataAssetSettings>().AssetRef == null)
                {
                    ScriptableRef.GetAssetDef<DataAssetSettings>().TryCreate();
                }
                
                AssetIndexHandler.UpdateIndex();
                SaveManager.SaveGame();
                Debug.LogError("Save init from import...");
                onComplete?.Invoke();
            }
        }
    }
}