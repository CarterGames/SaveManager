/*
 * Save Manager
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

using System;
using CarterGames.Shared.SaveManager;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles updating the save location of the users save data if it has changed since the last runtime.
    /// </summary>
    /// <remarks>
    /// May not work 100% of the time. Best to keep the save location the same throughout the development of a project really.
    /// </remarks>
    public static class SaveLocationChangeHandlerRuntime
    {
        private static string RuntimeLocationKey = "rt_last_save_location";


        private static string LastSaveLocation
        {
            get => SaveManagerPrefs.GetStringKey(RuntimeLocationKey);
            set => SaveManagerPrefs.SetKey(RuntimeLocationKey, value);
        }


        private static ISaveDataLocation CurrentSaveLocation =>
            SmAssetAccessor.GetAsset<DataAssetSettings>().Location;
        
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
#if !UNITY_EDITOR
            // Has no previous save location stored.
            if (!TryParseLocationType(LastSaveLocation, out var location))
            {
                // Save current as last
                LastSaveLocation = JsonUtility.ToJson(SmAssetAccessor.GetAsset<DataAssetSettings>().LocationClassDef);
                return;
            }

            // If the same as the current, skip the rest.
            if (location == CurrentSaveLocation) return;
                
            // Save data from old to new.
            var data = JsonUtility.FromJson<AssemblyClassDef>(LastSaveLocation).GetDefinedType<ISaveDataLocation>().LoadDataFromLocation();
            CurrentSaveLocation.SaveDataToLocation(data);
            
            // Update last location used.
            LastSaveLocation = JsonUtility.ToJson(SmAssetAccessor.GetAsset<DataAssetSettings>().LocationClassDef);
#endif
        }
        
        
        private static bool TryParseLocationType(string value, out ISaveDataLocation saveLocation)
        {
            try
            {
                saveLocation = JsonUtility.FromJson<AssemblyClassDef>(value).GetDefinedType<ISaveDataLocation>();
                return saveLocation != null;
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                saveLocation = null;
                return false;
            }
        }
    }
}