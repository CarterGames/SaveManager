using System;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class EditorSaveInitializer
    {
        private static bool HasInitialized
        {
            get => EditorUserSettings.GetBool($"{SaveManagerConstants.PrefFormat}_initial_editor_init", false);
            set => EditorUserSettings.SetBool($"{SaveManagerConstants.PrefFormat}_initial_editor_init", value);
        }


        public static void TryInitializeAsset(Action onComplete)
        {
            if (HasInitialized)
            {
                onComplete?.Invoke();
                return;
            }

            if (EditorSaveObjectController.IsInitialized)
            {
                onComplete?.Invoke();
                return;
            }
            
            EditorSaveObjectController.InitializedEditorEvt.Add(OnSaveObjectInit);
            EditorSaveObjectController.Initialize();
            return;

            void OnSaveObjectInit()
            {
                onComplete?.Invoke();
                HasInitialized = true;
            }
        }
    }
}