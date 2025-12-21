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

using CarterGames.Assets.SaveManager.Backups;
using CarterGames.Assets.SaveManager.Legacy;
using CarterGames.Assets.SaveManager.Saving;
using CarterGames.Assets.SaveManager.Slots;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// The main Save Manager class to access the save system through.
    /// </summary>
    public static partial class SaveManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Used to lock the save system from running while an operation is active.
        /// </summary>
        private static readonly object SaveLock = new object();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The last Json data loaded from the save location.
        /// </summary>
        private static JToken LastLoadedSaveJson { get; set; }
        
        
        /// <summary>
        /// Gets if the save system is initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; }
        
        
        /// <summary>
        /// Gets if the save system is currently saving the game.
        /// </summary>
        public static bool IsSaving { get; private set; }
        
        
        /// <summary>
        /// Gets if the save system is currently loading the game.
        /// </summary>
        public static bool IsLoading { get; private set; }


        /// <summary>
        /// The current save location handler from the settings.
        /// </summary>
        private static ISaveDataLocation LocationHandler => SmAssetAccessor.GetAsset<DataAssetSettings>().Location;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises when the save manager is initialized.
        /// </summary>
        public static readonly Evt InitializedEvt = new Evt();
        
        
        /// <summary>
        /// Raises when the game has started a save operation.
        /// </summary>
        public static readonly Evt GameSaveStartedEvt = new Evt();
        
        
        /// <summary>
        /// Raises when the game has completed a save operation.
        /// </summary>
        public static readonly Evt GameSaveCompletedEvt = new Evt();
        
        
        /// <summary>
        /// Raises when the game has started a load operation.
        /// </summary>
        public static readonly Evt GameLoadStartedEvt = new Evt();
        
        
        /// <summary>
        /// Raises when the game has completed a load operation.
        /// </summary>
        public static readonly Evt GameLoadedEvt = new Evt();
        
        
        /// <summary>
        /// Raises when the game has failed a load operation, but hasn't failed trying a backup yet.
        /// </summary>
        public static readonly Evt<LoadFailInfo> GameLoadFailedEvt = new Evt<LoadFailInfo>();
        
        
        /// <summary>
        /// Raises when the game has failed a load operation entirely, really bad :(
        /// </summary>
        public static readonly Evt GameLoadFailedCompletelyEvt = new Evt();

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initialized the save manager when called.
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            if (IsInitialized) return;
            
            InitializeListeners();
            
            IsInitialized = true;
            InitializedEvt.Raise();
        }
        
        
        /// <summary>
        /// Saves the game data to the save location in its current state when called.
        /// </summary>
        public static void SaveGame()
        {
            Initialize();
            
            if (IsSaving || IsLoading) return;

            lock (SaveLock)
            {
                IsSaving = true;
                GameSaveStartedEvt.Raise();

                // Get save data.
                var saveStructure = SaveStructure.GenerateSaveData();
                
                // Encrypt if needed.
                if (TryEncryptSaveContent(saveStructure["$content"].ToString(), out var encryptedContent))
                {
                    saveStructure["$content"] = encryptedContent;
                }
                
                // Save to location.
                LocationHandler.SaveDataToLocation(saveStructure.ToString());
                
                // Update last used location to current handler.
                PlayerPrefs.SetString(SaveManagerConstants.LastSaveLocationPrefKey, LocationHandler.GetType().FullName);

                if (!Application.isPlaying)
                {
                    SmDebugLogger.LogDev("Game data successfully saved.");
                }
                else
                {
                    SmDebugLogger.Log("Game data successfully saved.");
                }
                
                IsSaving = false;
                GameSaveCompletedEvt.Raise();
            }
        }


        /// <summary>
        /// Loads the game data from the save location when called.
        /// </summary>
        public static void LoadGame()
        {
            Initialize();
            
            if (IsSaving || IsLoading) return;

            lock (SaveLock)
            {
                IsLoading = true;
                GameLoadStartedEvt.Raise();
                
                // Load from location.
                var loadedSaveData = LocationHandler.LoadDataFromLocation();
                
                // Try load the current data.
                if (!TryLoadData(loadedSaveData))
                {
                    if (!SaveBackupManager.TryRestoreFromBackups())
                    {
                        IsLoading = false;
                        GameLoadFailedCompletelyEvt.Raise();
                    }
                    
                    IsLoading = false;
                    GameLoadedEvt.Raise();
                    return;
                }
                
                // Make backup as load was successful.
                SaveBackupManager.BackupLastLoadedData(LastLoadedSaveJson);
                
                if (!Application.isPlaying)
                {
                    SmDebugLogger.LogDev("Game data successfully loaded.");
                }
                else
                {
                    SmDebugLogger.Log("Game data successfully loaded.");
                }
                
                IsLoading = false;
                GameLoadedEvt.Raise();
            }
        }


        /// <summary>
        /// Tries to load the game data into the save system for use.
        /// </summary>
        /// <param name="data">The Json to load.</param>
        /// <returns>If the load was successful.</returns>
        public static bool TryLoadData(string data)
        {
            // Pre-deserialize edits.
            if (PreLoadLogicHandler.TryProcessAllHandlers(data, out var processedData))
            {
                data = processedData;
            }
            
            LastLoadedSaveJson = (JObject)JsonConvert.DeserializeObject(data, JsonHelper.SaveManagerSerializerSettings);
            
            // Decrypt if encrypted.
            if (SmAssetAccessor.GetAsset<DataAssetSettings>().EncryptSave)
            {
                if (!TryDecryptSaveContent(LastLoadedSaveJson["$content"].ToString(), out var decryptedContent)) return false;
                LastLoadedSaveJson["$content"] = (JObject)JsonConvert.DeserializeObject(decryptedContent, JsonHelper.SaveManagerSerializerSettings);
                SmDebugLogger.LogDev($"Save decrypted successfully as:\n{LastLoadedSaveJson["$content"]}");
            }
                
            // Apply data to project.
            if (LastLoadedSaveJson != null)
            {
                // Apply any legacy save data the user has at this point if applicable.
                if (LegacySaveManager.TryLoadLegacySaveData(LastLoadedSaveJson, out JToken updatedData))
                {
                    LastLoadedSaveJson = updatedData;
                }

                // Apply global data
                var globalData = LastLoadedSaveJson["$content"]["$global"].Value<JArray>();

                foreach (var entry in SaveObjectController.GlobalSaveObjects)
                {
                    entry.Load(globalData);
                }

                // Apply slot data
                if (SmAssetAccessor.GetAsset<DataAssetSettings>().UseSaveSlots)
                {
                    SaveSlotManager.INTERNAL_LoadSlotDataFromSave(LastLoadedSaveJson);
                }
            }

            return true;
        }
    }
}