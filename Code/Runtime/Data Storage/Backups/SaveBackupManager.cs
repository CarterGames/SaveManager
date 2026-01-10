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
using System.Linq;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Backups
{
    /// <summary>
    /// Handles the save backup generation when the game loads successfully.
    /// </summary>
    public static class SaveBackupManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Makes a backup from the last loaded data.
        /// </summary>
        /// <param name="data">The data loaded.</param>
        public static void BackupLastLoadedData(JToken data)
        {
            if (data == null)
            {
                SmDebugLogger.LogDev("Save Backup Manager: Cannot backup as data is null.");
                return;
            }
            
            var handler = SmAssetAccessor.GetAsset<DataAssetSettings>().BackupLocation;
            var firstBackup = handler.GetBackups().FirstOrDefault();
            
            // Avoids making a backup if the data is exactly the same as before.
            if (firstBackup != null)
            {
                if (firstBackup.SelectToken("json") != null)
                {
                    if (firstBackup["json"].ToString() == data.ToString())
                    {
                        SmDebugLogger.LogDev(
                            "Save Backup Manager: Will not make a backup as the data matches the last loaded already.");
                        return;
                    }
                }
            }

            handler.BackupData(data);
        }


        /// <summary>
        /// Tries to restore the save from any of the backups that'll load.
        /// </summary>
        /// <remarks>
        /// Isn't always going to work, but it will attempt to load any previous that did load successfully.
        /// </remarks>
        /// <returns>If it was successful or not.</returns>
        public static bool TryRestoreFromBackups()
        {
            var backups = SmAssetAccessor.GetAsset<DataAssetSettings>().BackupLocation.GetBackups().ToArray();
            
            for (var i = 0; i < SmAssetAccessor.GetAsset<DataAssetSettings>().MaxBackups; i++)
            {
                try
                {
                    // Try load any older save
                    if (SaveManager.TryLoadData(backups[i].ToString()))
                    {
                        return true;
                    }
                }
#pragma warning disable 0168
                catch (Exception e)
#pragma warning restore 0168
                {
                    SmDebugLogger.LogWarning(SaveManagerErrorCode.BackupRestoreFailed.GetErrorMessageFormat());
                    continue;
                }
            }
            
            return false;
        }

        
        /// <summary>
        /// Loads a backup directly from its data.
        /// </summary>
        /// <param name="entry">The entry to load from.</param>
        public static void LoadBackup(JToken entry)
        {
            try
            {
                SaveManager.TryLoadData(entry.ToString());
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore 0168
            {
                SmDebugLogger.LogWarning(SaveManagerErrorCode.BackupRestoreFailed.GetErrorMessageFormat());
            }
        }
    }
}