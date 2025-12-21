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

using CarterGames.Shared.SaveManager;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles the auto saving of the game if the setting is enabled.
    /// </summary>
    public class AutoSaveOnExit : MonoBehaviour
    {
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Fields
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        private static AutoSaveOnExit instance;

        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Properties
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// Gets if the save has happened already.
        /// </summary>
        private static bool HasSaved { get; set; }
        
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Methods
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// Initializes the auto saving setup for use if the setting is enabled.
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void SetupInstance()
        {
            if (!SmAssetAccessor.GetAsset<DataAssetSettings>().AutoSave) return;
            if (instance != null) return;

            var obj = new GameObject("[DND] [Save Manager] Auto Save Behaviour");
            obj.AddComponent<AutoSaveOnExit>();
            instance = obj.GetComponent<AutoSaveOnExit>();
            DontDestroyOnLoad(obj);
        }

        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Unity Methods
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// Runs when the application quits
        /// </summary>
        private void OnDestroy()
        {
            if (!SmAssetAccessor.GetAsset<DataAssetSettings>().AutoSave) return;
            if (HasSaved) return;
            SaveManager.SaveGame();
        }


        /// <summary>
        /// Runs when the application loses focus, like clicking off the game application when its running. 
        /// </summary>
        /// <param name="hasFocus">If the user is focused on the application.</param>
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!SmAssetAccessor.GetAsset<DataAssetSettings>().AutoSave) return;
            if (HasSaved) return;
            
            if (hasFocus)             
            {
                HasSaved = false;
                return;
            }
            
            SaveManager.SaveGame();
        }


        private void OnApplicationQuit()
        {
            if (!SmAssetAccessor.GetAsset<DataAssetSettings>().AutoSave) return;
            if (HasSaved) return;
            SaveManager.SaveGame();
        }
    }
}