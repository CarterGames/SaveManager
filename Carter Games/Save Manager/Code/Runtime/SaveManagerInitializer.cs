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
    /// Handles calling the relevant systems to initialize the save manager at runtime.
    /// </summary>
    public static class SaveManagerInitializer
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public static bool IsInitialized { get; private set; }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public static Evt InitializedEvt = new Evt();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Runs Save Manager initialization when called.
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        public static void InitializeSaveManagerRuntimeSetup()
        {
            if (IsInitialized) return;
            if (SaveObjectController.IsInitialized) return;
            
            SaveObjectController.InitializedEvt.Add(OnSaveObjectsInitialized);
            
            SaveObjectController.Initialize();
            return;

            void OnSaveObjectsInitialized()
            {
                SaveObjectController.InitializedEvt.Remove(OnSaveObjectsInitialized);
                SaveManager.GameLoadedEvt.Add(OnGameLoaded);
                
                SaveManager.LoadGame();
            }

            void OnGameLoaded()
            {
                SaveManager.GameLoadedEvt.Remove(OnGameLoaded);
                IsInitialized = true;
                InitializedEvt.Raise();
            }
        }
    }
}