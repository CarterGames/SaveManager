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
using Newtonsoft.Json.Linq;

namespace CarterGames.Assets.SaveManager.Legacy
{
    /// <summary>
    /// Converts any 2.x save data that has matching keys in the 3.x setup to the 3.x setup. But only if it is in global save space.
    /// </summary>
    public class LegacySaveHandlerGlobalOnly : ILegacySaveHandler
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Converts a legacy save to the new setup.
        /// </summary>
        /// <param name="loadedJson">The loaded JSON to work with (3.x).</param>
        /// <param name="legacyData">The legacy data to work with (2.x).</param>
        /// <returns>The updates json to continue loading from (3.x).</returns>
        public JToken ProcessLegacySaveData(JToken loadedJson, IReadOnlyDictionary<string, JArray> legacyData)
        {
            // Processes each save value based on if its key exists in the global save in 3.x
            // If its now slot data it'll not be transferred in this setup.
            // You'll have to your own handler if you want to load legacy to slots.
            JToken updated = loadedJson;

            var globalDataArray = loadedJson["$content"]["$global"].Value<JArray>();
            
            foreach (var entry in legacyData)
            {
                foreach (var legacySaveValue in entry.Value)
                {
                    var legacySaveValueKey = legacySaveValue["$key"].Value<string>();

                    for (var i = 0; i < globalDataArray.Count; i++)
                    {
                        var currentSaveValue = globalDataArray[i];

                        if (currentSaveValue["$key"].Value<string>() != legacySaveValueKey) continue;

                        var adjusted = currentSaveValue;
                        adjusted["$value"] = legacySaveValue["$value"];

                        if (legacySaveValue["$default"] != null)
                        {
                            adjusted["$default"] = legacySaveValue["$default"];
                        }
                        
                        updated["$content"]["$global"][i] = adjusted;

                        SmDebugLogger.LogDev($"Legacy data converted:\n{updated["$content"]["$global"][i]}");
                    }
                }
            }

            return updated;
        }
    }
}