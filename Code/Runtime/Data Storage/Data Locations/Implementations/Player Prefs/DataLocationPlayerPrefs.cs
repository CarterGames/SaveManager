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

using System.Collections.Generic;
using System.Linq;
using CarterGames.Assets.SaveManager.Helpers;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles storing data in player prefs.
    /// </summary>
    public sealed class DataLocationPlayerPrefs : IDataLocation
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string PlayerPrefKeyTemplate = "CarterGames_SaveManager_{0}_Chunk_{1}";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the pref key for the relevant chunk of the data.
        /// </summary>
        /// <param name="path">The path (pref key) to get from</param>
        /// <param name="index">The chunk index to get</param>
        /// <returns>The key for the requested chunk for use.</returns>
        private static string GetPrefKey(string path, int index)
        {
            return string.Format(PlayerPrefKeyTemplate, path, index);
        }
        
        
        /// <summary>
        /// Gets if the location has data currently stored in it.
        /// </summary>
        /// <param name="path">The path to store the data at.</param>
        /// <returns>If there is data in the location.</returns>
        public bool HasData(string path)
        {
            return PlayerPrefs.HasKey(GetPrefKey(path, 0));
        }

        
        /// <summary>
        /// Saves data to the location when called.
        /// </summary>
        /// <param name="path">The path to store the data at.</param>
        /// <param name="data">The data to store.</param>
        public void SaveToLocation(string path, string data)
        {
            ClearAllChunks(path);
            
            var chunks = PlayerPrefHelper.ConvertToChunkCollection(data).ToArray();

            for (var i = 0; i < chunks.Length; i++)
            {
                PlayerPrefs.SetString(GetPrefKey(path, i), chunks[i]);
            }
            
            PlayerPrefs.Save();
        }
        

        /// <summary>
        /// Loads the data from the location when called.
        /// </summary>
        /// <param name="path">The path to load the data at.</param>
        /// <returns>The data loaded from the location.</returns>
        public string LoadFromLocation(string path)
        {
            var data = new List<string>();
            var iteration = 0;

            while (PlayerPrefs.HasKey(GetPrefKey(path, iteration)))
            {
                data.Add(PlayerPrefs.GetString(GetPrefKey(path, iteration)));
                iteration++;
            }

            return PlayerPrefHelper.CovertCollectionToString(data);
        }


        /// <summary>
        /// Clears all the chunks from the location, so its empty.
        /// </summary>
        /// <param name="path">The path (pref key) to clear</param>
        private static void ClearAllChunks(string path)
        {
            var iteration = 0;

            while (PlayerPrefs.HasKey(GetPrefKey(path, iteration)))
            {
                PlayerPrefs.DeleteKey(GetPrefKey(path, iteration));
                iteration++;
            }
            
            PlayerPrefs.Save();
        }
    }
}