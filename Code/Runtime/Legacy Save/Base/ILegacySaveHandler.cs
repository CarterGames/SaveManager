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
    /// Implement to write a converter for a legacy 2.x save to turn in into a 3.x save structure.
    /// </summary>
    public interface ILegacySaveHandler
    {
        /// <summary>
        /// Converts a legacy save to the new setup.
        /// </summary>
        /// <param name="loadedJson">The loaded JSON to work with (3.x).</param>
        /// <param name="legacyData">The legacy data to work with (2.x).</param>
        /// <returns>The updates json to continue loading from (3.x).</returns>
        JToken ProcessLegacySaveData(JToken loadedJson, IReadOnlyDictionary<string, JArray> legacyData);
    }
}