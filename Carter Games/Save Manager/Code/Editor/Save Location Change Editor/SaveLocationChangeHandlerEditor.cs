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