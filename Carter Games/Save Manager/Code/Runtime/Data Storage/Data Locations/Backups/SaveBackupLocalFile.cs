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
using System.IO;
using System.Linq;
using CarterGames.Assets.SaveManager.Helpers;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CarterGames.Assets.SaveManager.Backups
{
    /// <summary>
    /// Handles storing game backups to a local file.
    /// </summary>
    public sealed class SaveBackupLocalFile : ISaveBackupLocation
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string BackupsLocation = "%Application.persistentDataPath%/SaveBackups/";
        private const string BackupsPath = "%Application.persistentDataPath%/SaveBackups/save_bak_{0}.sf2";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the parsed location for the save backup.
        /// </summary>
        private string ParsedBackupsLocation => LocalFileHelper.ParseSavePath(BackupsLocation);
        
        
        /// <summary>
        /// Gets the parsed path for the save backup.
        /// </summary>
        private string ParsedBackupsPath => LocalFileHelper.ParseSavePath(BackupsPath);
        
        
        /// <summary>
        /// Gets the location implementation to use with the setup.
        /// </summary>
        public IDataLocation Location => new DataLocationLocalFile();
        
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
                    Location.SaveToLocation(string.Format(ParsedBackupsPath, newIteration), new JObject()
                    {
                        ["iteration"] = newIteration,
                        ["json"] = backup["json"],
                    }.ToString());
                }
            }

            Location.SaveToLocation(string.Format(ParsedBackupsPath, 0), new JObject()
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
            if (!Directory.Exists(ParsedBackupsLocation)) return Array.Empty<JObject>();

            var files = Directory.GetFiles(ParsedBackupsLocation);
            var loadedData = new JObject[files.Length];
            
            for (var i = 0; i < files.Length; i++)
            {
                var filePath = string.Format(ParsedBackupsPath, i);
                loadedData[i] = (JObject)JsonConvert.DeserializeObject(Location.LoadFromLocation(filePath), new JsonSerializerSettings()
                {
                    DateParseHandling = DateParseHandling.None
                });
            }

            return loadedData;
        }
    }
}