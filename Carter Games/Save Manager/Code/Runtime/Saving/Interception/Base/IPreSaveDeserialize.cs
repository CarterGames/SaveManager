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
    /// Implement to add logic before the save data before it is loaded.
    /// </summary>
    public interface IPreSaveDeserialize
    {
        /// <summary>
        /// The order it executes in.
        /// </summary>
        int DeserializeExecutionOrder { get; }
        
        
        /// <summary>
        /// Is called before the save deserializes into the system, so you can make edits if needed.
        /// </summary>
        /// <param name="data">The data from the save.</param>
        /// <returns>The edited data from the method.</returns>
        string OnPreSaveDeserialize(string data);
    }
}