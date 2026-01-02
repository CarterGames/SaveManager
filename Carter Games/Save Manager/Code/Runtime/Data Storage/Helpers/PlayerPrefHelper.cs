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

namespace CarterGames.Assets.SaveManager.Helpers
{
    /// <summary>
    /// A helper class for player prefs.
    /// </summary>
    public static class PlayerPrefHelper
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const int MaxChunkSize = 75; // Technically 100 is the max, 75 is just for safety xD
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Converts a json blob to chunks for player prefs to save with.
        /// </summary>
        /// <param name="json">The json to convert.</param>
        /// <returns>A collection of string from the json entered.</returns>
        public static IEnumerable<string> ConvertToChunkCollection(string json)
        {
            var iterations = json.Length / MaxChunkSize + 1;
            var chunkArray = new string[iterations];
            var data = new string(json.ToCharArray()).Trim(' ');

            for (var i = 0; i < iterations; i++)
            {
                var takenData = new string(data.Take(MaxChunkSize).ToArray());
                chunkArray[i] = takenData;

                var totalToRemove = data.Length >= MaxChunkSize ? MaxChunkSize : data.Length - 1;
                
                if (totalToRemove <= 0) break;

                data = data.Remove(0, totalToRemove);
            }

            return chunkArray;
        }


        /// <summary>
        /// Converts a collection of strings (chunks) back to a json blob.
        /// </summary>
        /// <param name="data">The data to convert.</param>
        /// <returns>The json blob generated.</returns>
        public static string CovertCollectionToString(IEnumerable<string> data)
        {
            var result = string.Empty;

            foreach (var entry in data)
            {
                result += entry;
            }
            
            return result;
        }
    }
}