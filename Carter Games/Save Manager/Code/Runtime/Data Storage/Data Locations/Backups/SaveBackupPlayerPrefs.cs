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

using System;
using System.Collections.Generic;
using System.Linq;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Backups
{
    /// <summary>
    /// Handles storing game backups to a player prefs.
    /// </summary>
    public sealed class SaveBackupPlayerPrefs : ISaveBackupLocation
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string Path = "SaveBackup_{0}";
        private const string TotalBackupsKey = "CarterGames_SaveManager_PlayerPrefsSaveBackups_TotalBackups";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the location implementation to use with the setup.
        /// </summary>
        public IDataLocation Location => new DataLocationPlayerPrefs();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Backs up the data when called.
        /// </summary>
        /// <param name="data">The data to backup.</param>
        public void BackupData(JToken data)
        {
            var currentBackups = GetBackups();
            
            if (currentBackups.Any())
            {
                foreach (var backup in currentBackups)
                {
                    var newIteration = backup["iteration"].Value<int>() + 1;
            
                    // Trim any extra backups off the list of saved ones.
                    if (newIteration >= SmAssetAccessor.GetAsset<DataAssetSettings>().MaxBackups) continue;
                    Location.SaveToLocation(string.Format(Path, newIteration), new JObject()
                    {
                        ["iteration"] = newIteration,
                        ["json"] = backup["json"],
                    }.ToString());
                }
            }

            Location.SaveToLocation(string.Format(Path, 0), new JObject()
            {
                ["iteration"] = 0,
                ["json"] = data,
            }.ToString());
        }
        

        /// <summary>
        /// Gets the backups stored for use.
        /// </summary>
        /// <returns>The backups found.</returns>
        public IEnumerable<JObject> GetBackups()
        {
            var totalBackups = PlayerPrefs.GetInt(TotalBackupsKey);

            if (totalBackups <= 0) return Array.Empty<JObject>();
            
            var loadedData = new JObject[totalBackups];
            
            for (var i = 0; i < totalBackups; i++)
            {
                loadedData[i] = (JObject)JsonConvert.DeserializeObject(Location.LoadFromLocation(Path), new JsonSerializerSettings()
                {
                    DateParseHandling = DateParseHandling.None
                });
            }

            return loadedData;
        }
    }
}