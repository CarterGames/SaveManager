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

using System.Collections.Generic;
using System.Linq;
using CarterGames.Assets.SaveManager.Helpers;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles storing data in player prefs.
    /// </summary>
    public class DataLocationPlayerPrefs : IDataLocation
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string PlayerPrefKeyTemplate = "CarterGames_SaveManager_{0}_Chunk_{1}";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private string GetPrefKey(string path, int index)
        {
            return string.Format(PlayerPrefKeyTemplate, path, index);
        }
        
        
        public bool HasData(string path)
        {
            return PlayerPrefs.HasKey(GetPrefKey(path, 0));
        }

        
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


        private void ClearAllChunks(string path)
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