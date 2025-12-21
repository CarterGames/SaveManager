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
    /// Implement to define a location to store data.
    /// </summary>
    public interface IDataLocation
    {
        /// <summary>
        /// Gets if the location has data currently stored in it.
        /// </summary>
        /// <param name="path">The path to store the data at.</param>
        /// <returns>If there is data in the location.</returns>
        bool HasData(string path);
        
        
        /// <summary>
        /// Saves data to the location when called.
        /// </summary>
        /// <param name="path">The path to store the data at.</param>
        /// <param name="data">The data to store.</param>
        void SaveToLocation(string path, string data);
        
        
        /// <summary>
        /// Loads the data from the location when called.
        /// </summary>
        /// <param name="path">The path to load the data at.</param>
        /// <returns>The data loaded from the location.</returns>
        string LoadFromLocation(string path);
    }
}