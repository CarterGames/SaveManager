using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class SaveLocationChangeHandlerEditor
    {
        [InitializeOnLoadMethod]
        private static void EditorInitialize()
        {
            SaveManagerSettingsProvider.SaveLocationChangedEvt.Add(OnSaveLocationSettingChanged);
        }


        private static void OnSaveLocationSettingChanged(ISaveDataLocation previousLocation, ISaveDataLocation currentLocation)
        {
            // If no previous assigned, skip moving data as they'd be nothing to move to the new setup.
            if (previousLocation == null) return;
            
            var data = previousLocation.LoadDataFromLocation();
            currentLocation.SaveDataToLocation(data);
        }
    }
}