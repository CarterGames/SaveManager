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

using System.Linq;
using CarterGames.Assets.SaveManager.Slots;

namespace CarterGames.Assets.SaveManager
{
    // An extension for the Save Value API.
    public static partial class SaveManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets a save value in the desired context.
        /// </summary>
        /// <param name="saveKey">The save key to find.</param>
        /// <param name="ctx">The ctx to narrow the search.</param>
        /// <typeparam name="T">The save value type to get.</typeparam>
        /// <returns>SaveValue</returns>
        private static SaveValue<T> GetSaveValue<T>(string saveKey, SaveCtx ctx = SaveCtx.Unassigned)
        {
            if (ctx.HasFlag(SaveCtx.Unassigned))
            {
                // Not possible to get a save value...
                return null;
            }

            SaveValue<T> result = null;
            
            // Global
            if (ctx.HasFlag(SaveCtx.Global))
            {
                result = (SaveValue<T>) SaveObjectController.AllGlobalSaveValues
                    .FirstOrDefault(t => t.Value
                        .Any(x => x.key == saveKey))
                    .Value.FirstOrDefault(t => t.key == saveKey);

                if (result != null) return result;
            }
            
            
            // Active slot
            if (ctx.HasFlag(SaveCtx.ActiveSlot))
            {
                result = (SaveValue<T>) SaveObjectController.AllSlotSaveValues[SaveSlotManager.ActiveSlot]
                    .FirstOrDefault(t => t.Value
                        .Any(x => x.key == saveKey))
                    .Value.FirstOrDefault(t => t.key == saveKey);
                
                if (result != null) return result;
            }

            return result;
        }
        
        
        /// <summary>
        /// Tries to get a save value in the desired context.
        /// </summary>
        /// <param name="saveKey">The save key to find.</param>
        /// <param name="ctx">The ctx to narrow the search.</param>
        /// <param name="value">The SaveValue found.</param>
        /// <typeparam name="T">The save value type to get.</typeparam>
        /// <returns>Bool</returns>
        public static bool TryGetSaveValue<T>(string saveKey, out SaveValue<T> value, SaveCtx ctx = SaveCtx.Unassigned)
        {
            value = GetSaveValue<T>(saveKey, SaveCtx.All);

            if (value == null)
            {
                SmDebugLogger.LogError(SaveManagerErrorCode.NoSaveValueFound.GetErrorMessageFormat(saveKey, nameof(T)));
            }
            
            return value != null;
        }
        
        
        /// <summary>
        /// Tries to get a save value in the global context only.
        /// </summary>
        /// <param name="saveKey">The save key to find.</param>
        /// <param name="value">The SaveValue found.</param>
        /// <typeparam name="T">The save value type to get.</typeparam>
        /// <returns>Bool</returns>
        public static bool TryGetGlobalSaveValue<T>(string saveKey, out SaveValue<T> value)
        {
            value = GetSaveValue<T>(saveKey, SaveCtx.Global);
            
            if (value == null)
            {
                SmDebugLogger.LogError(SaveManagerErrorCode.NoSaveValueFound.GetErrorMessageFormat(saveKey, nameof(T)));
            }
            
            return value != null;
        }
        
        
        /// <summary>
        /// Tries to get a save value in the active slot context only.
        /// </summary>
        /// <param name="saveKey">The save key to find.</param>
        /// <param name="value">The SaveValue found.</param>
        /// <typeparam name="T">The save value type to get.</typeparam>
        /// <returns>Bool</returns>
        public static bool TryGetActiveSlotSaveValue<T>(string saveKey, out SaveValue<T> value)
        {
            value = GetSaveValue<T>(saveKey, SaveCtx.ActiveSlot);
            
            if (value == null)
            {
                SmDebugLogger.LogError(SaveManagerErrorCode.NoSaveValueFound.GetErrorMessageFormat(saveKey, nameof(T)));
            }
            
            return value != null;
        }
    }
}