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
using CarterGames.Assets.SaveManager.Backups;
using CarterGames.Assets.SaveManager.Encryption;
using CarterGames.Assets.SaveManager.Helpers;
using CarterGames.Assets.SaveManager.Legacy;
using CarterGames.Shared.SaveManager;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles the runtime settings for the save manager asset.
    /// </summary>
    [CreateAssetMenu]
    [Serializable]
    public sealed class DataAssetSettings : SmDataAsset
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

#pragma warning disable 
        [SerializeField] private string defaultSavePath = "%Application.persistentDataPath%/save.sf2";
        [SerializeField] private string defaultSavePathWeb = "/idbfs/%productName%-%companyName%/save.sf2";
#pragma warning restore
        
        [SerializeField] [Range(1,10)] private int maxBackups = 1;
        
        [SerializeField] private bool autoSaveOnExit = true;
        
        [SerializeField] private AssemblyClassDef saveLocation = AssemblyClassDef.FromType<SaveLocationLocalFile>();
        [SerializeField] private AssemblyClassDef backupLocation = AssemblyClassDef.FromType<SaveBackupLocalFile>();

        [SerializeField] private bool useJsonConverters = true;
        
        [SerializeField] private bool encryptSave = false;
        [SerializeField] private AssemblyClassDef encryptionHandler;
        
        [SerializeField] private bool tryPortLegacySave = true;
        [SerializeField] private AssemblyClassDef legacySaveHandler = AssemblyClassDef.FromType<LegacySaveHandlerGlobalOnly>();
        
        [SerializeField] private bool useSaveSlots;
        [SerializeField] private bool limitAvailableSlots;
        [SerializeField] [Range(1, 100)] private int maxUserSaveSlots = 1;

        [SerializeField] private bool useMetaDataGameInfo = true;
        [SerializeField] private bool useMetaDataSystemInfo = true;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets the save path for the game save data.
        /// </summary>
        public string SavePath
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return LocalFileHelper.ParseSavePath(ParseSavePath);
#elif UNITY_EDITOR
                return LocalFileHelper.ParseSavePath("%Application.persistentDataPath%/EditorSave/save.sf2");
#else
                return LocalFileHelper.ParseSavePath(defaultSavePath);
#endif
            }
        }
        
        
        /// <summary>
        /// The save location in use to store the game save at.
        /// </summary>
        public ISaveDataLocation Location => saveLocation?.GetDefinedType<ISaveDataLocation>();
        
        
        /// <summary>
        /// Gets the save location class define for edits in editor space mainly.
        /// </summary>
        public AssemblyClassDef LocationClassDef => saveLocation;
        
        
        /// <summary>
        /// The backup location in use to store game save backups when it successfully loads a save.
        /// </summary>
        public ISaveBackupLocation BackupLocation => backupLocation?.GetDefinedType<ISaveBackupLocation>();
        
        
        /// <summary>
        /// Gets if the save should be encrypted.
        /// </summary>
        public bool EncryptSave => encryptSave;
        
        
        /// <summary>
        /// Gets if there is an encryption handler assigned in the settings.
        /// </summary>
        public bool HasEncryptionHandler => encryptionHandler.IsValid;
        
        
        /// <summary>
        /// Gets the encrypted handler assigned to encrypt & decrypt the game save content.
        /// </summary>
        public ISaveEncryptionHandler EncryptionHandler => encryptionHandler?.GetDefinedType<ISaveEncryptionHandler>();

        
        /// <summary>
        /// Gets if the save should try to port any 2.x saves detected.
        /// </summary>
        public bool TryPortLegacySave => tryPortLegacySave;
        
        
        /// <summary>
        /// Gets if the system has a legacy save converter assigned.
        /// </summary>
        public bool HasLegacySaveHandler => legacySaveHandler.IsValid;
        
        
        /// <summary>
        /// Gets the assigned legacy save converted to use.
        /// </summary>
        public ILegacySaveHandler LegacySaveHandler => legacySaveHandler?.GetDefinedType<ILegacySaveHandler>();
        
        
        /// <summary>
        /// Gets the max backups to make before overriding previous backups by age.
        /// </summary>
        public int MaxBackups => maxBackups;


        /// <summary>
        /// Defines if the asset auto saves changes.
        /// </summary>
        public bool AutoSave => autoSaveOnExit;
        
        
        /// <summary>
        /// Defines if save slots are used.
        /// </summary>
        public bool UseSaveSlots => useSaveSlots;
        
        
        /// <summary>
        /// Defines if the amount of save slots available are limited.
        /// </summary>
        public bool LimitAvailableSlots => limitAvailableSlots;
        
        
        /// <summary>
        /// Defines the max save slots available to the save system. 
        /// </summary>
        public int MaxUserSaveSlots => maxUserSaveSlots;


        /// <summary>
        /// Gets if the save system should use the asset built-in json converters.
        /// </summary>
        public bool UseJsonConverters => useJsonConverters;
        
        
        /// <summary>
        /// Defines if the game info metadata is assigned to the game save.
        /// </summary>
        public bool UseMetaDataGameInfo => useMetaDataGameInfo;
        
        
        /// <summary>
        /// Defines if the system info metadata is assigned to the game save.
        /// </summary>
        public bool UseMetaDataSystemInfo => useMetaDataSystemInfo;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnValidate()
        {
            if (defaultSavePath.Length > 0) return;
            
            defaultSavePath = "%Application.persistentDataPath%/save.sf";
            defaultSavePathWeb = "/idbfs/%productName%-%companyName%/save.sf";
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initializes the asset when called.
        /// </summary>
        public void Initialize()
        {
            OnValidate();
        }
    }
}