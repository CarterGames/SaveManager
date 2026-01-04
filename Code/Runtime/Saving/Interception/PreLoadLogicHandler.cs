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

using System;
using System.Linq;
using CarterGames.Shared.SaveManager;

namespace CarterGames.Assets.SaveManager.Saving
{
    /// <summary>
    /// Handles running logic before the game load happens.
    /// </summary>
    public static class PreLoadLogicHandler
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static IPreSaveDeserialize[] handlers = Array.Empty<IPreSaveDeserialize>();

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Processes all handlers in existence in the project.
        /// </summary>
        /// <param name="loadedSaveData">The data to process.</param>
        /// <param name="processedData">The modified data to now use.</param>
        /// <returns>Bool, if any errors occurred it'd return false.</returns>
        public static bool TryProcessAllHandlers(string loadedSaveData, out string processedData)
        {
            processedData = loadedSaveData;
            
            // Get handlers if the collection is currently empty. 
            if (handlers.Length <= 0)
            {
                handlers = AssemblyHelper
                    .GetClassesOfType<IPreSaveDeserialize>()
                    .OrderBy(t => t.DeserializeExecutionOrder)
                    .ToArray();
            }

            if (handlers.Length <= 0) return true;

            var data = loadedSaveData;
            
            foreach (var entry in handlers)
            {
                try
                {
                    data = entry.OnPreSaveDeserialize(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            processedData = data;
            return true;
        }
    }
}