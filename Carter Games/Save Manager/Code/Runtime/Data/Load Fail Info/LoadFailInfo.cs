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

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A data class for any game load fail issues.
    /// </summary>
    public sealed class LoadFailInfo
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The message for the load fail.
        /// </summary>
        public string Message { get; private set; }
        
        
        /// <summary>
        /// The data that failed to load.
        /// </summary>
        public string LoadData { get; private set; }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Makes a new load fail class.
        /// </summary>
        /// <param name="data">The data from the load.</param>
        /// <param name="message">The message about the issue.</param>
        public LoadFailInfo(string data, string message = "")
        {
            Message = message;
            LoadData = data;
        }
    }
}