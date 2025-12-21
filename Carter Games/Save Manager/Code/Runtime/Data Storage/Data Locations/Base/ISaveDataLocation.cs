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

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Implement to define a save data location to store the game save at.
    /// </summary>
    /// <remarks>
    /// Requires an IDataLocation implementation to go along with it if it is at a new location setup.
    /// </remarks>
    public interface ISaveDataLocation
    {
        /// <summary>
        /// The data location to use for the game save data.
        /// </summary>
        IDataLocation DataLocation { get; }
        
        
        /// <summary>
        /// Gets if the location has a save data already.
        /// </summary>
        bool HasSaveData { get; }
        
        
        /// <summary>
        /// Saves the data to the location defined in <see cref="DataLocation"/>
        /// </summary>
        /// <param name="json">The json to save to the location.</param>
        void SaveDataToLocation(string json);
        
        
        /// <summary>
        /// Loads the data from the location defined in <see cref="DataLocation"/>
        /// </summary>
        /// <returns>The data loaded from the location.</returns>
        string LoadDataFromLocation();
    }
}