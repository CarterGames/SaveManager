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

using Newtonsoft.Json.Linq;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Implemented to define a meta data element of the save.
    /// </summary>
    public interface ISaveMetaData
    {
        /// <summary>
        /// The key for the metadata to be stored under.
        /// </summary>
        string Key { get; }
        
        
        /// <summary>
        /// Gets if the metadata can be written to the save.
        /// </summary>
        bool CanWriteMetaData { get; }
        
        
        /// <summary>
        /// Gets the metadata to write to the save.
        /// </summary>
        /// <returns>A JObject with the data to write.</returns>
        JObject GetMetaData();
    }
}