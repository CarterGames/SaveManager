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
    /// Defines an load listener implementation.
    /// </summary>
    public interface IGameLoadListener
    {
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Methods
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// Implement to run logic when the save is called to load at runtime.
        /// </summary>
        void OnGameLoadCalled();


        /// <summary>
        /// Implement to run logic when the save has failed to load the last saved data.
        /// </summary>
        void OnGameLoadFailed(LoadFailInfo loadFailInfo);
        
        
        /// <summary>
        /// Implement to run logic when the save has failed to load (includes trying backups)
        /// </summary>
        void OnGameLoadFailedCompletely();
        

        /// <summary>
        /// Implement to run logic when the save has completed loading at runtime.
        /// </summary>
        void OnGameLoadCompleted();
    }
}