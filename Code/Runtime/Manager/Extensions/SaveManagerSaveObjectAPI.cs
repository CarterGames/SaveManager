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
    // An extension for the Save Object API.
    public static partial class SaveManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets a global save object from the save setup.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>Save Object</returns>
        public static T GetGlobalSaveObject<T>() where T : SaveObject
        {
            return (T) SaveObjectController.GlobalSaveObjects.FirstOrDefault(t => t.GetType() == typeof(T));
        }


        /// <summary>
        /// Tries to get a global save object from the save setup.
        /// </summary>
        /// <param name="saveObject">The save object found.</param>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>If it was successful or not.</returns>
        public static bool TryGetGlobalSaveObject<T>(out T saveObject) where T : SaveObject
        {
            saveObject = GetGlobalSaveObject<T>();

            if (saveObject == null)
            {
                SmDebugLogger.Log(SaveManagerErrorCode.NoGlobalSaveObjectOfType.GetErrorMessageFormat(nameof(T)));
                return false;
            }

            return true;
        }


        /// <summary>
        /// Gets a slot save object from the active save slot in the save setup.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>Slot Save Object</returns>
        public static T GetActiveSlotSaveObject<T>() where T : SlotSaveObject
        {
            return GetSlotSaveObject<T>(SaveSlotManager.ActiveSlotIndex);
        }


        /// <summary>
        /// Tries to get a slot save object from the active save slot in the save setup.
        /// </summary>
        /// <param name="slotSaveObject">The slot save object found.</param>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>If it was successful or not.</returns>
        public static bool TryGetActiveSlotSaveObject<T>(out T slotSaveObject) where T : SlotSaveObject
        {
            slotSaveObject = GetActiveSlotSaveObject<T>();

            if (slotSaveObject == null)
            {
                SmDebugLogger.Log(SaveManagerErrorCode.NoSlotSaveObjectOfType.GetErrorMessageFormat(nameof(T)));
                return false;
            }

            return true;
        }
        
        
        /// <summary>
        /// Gets a slot save object from a specific slot in the save setup.
        /// </summary>
        /// <param name="slotId">The slot id to get the slot save object from.</param>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>SlotSaveObject</returns>
        public static T GetSlotSaveObject<T>(int slotId) where T : SlotSaveObject
        {
            var slot = SaveObjectController.AllSlotSaveObjects.FirstOrDefault(t => t.Key.SlotId != slotId).Key;
            
            if (slot == null)
            {
                SmDebugLogger.Log(SaveManagerErrorCode.NoSlotSaveObjectOfType.GetErrorMessageFormat(nameof(T)));
                return null;
            }

            return (T) SaveObjectController.AllSlotSaveObjects[slot].SlotSaveObjects
                .FirstOrDefault(t => t.GetType() == typeof(T));
        }


        /// <summary>
        /// Tries to get a slot save object from a specific slot in the save setup.
        /// </summary>
        /// <param name="slotId">The slot id to get the slot save object from.</param>
        /// <param name="saveObject">The found SlotSaveObject</param>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>If it was successful or not.</returns>
        public static bool TryGetSlotSaveObject<T>(int slotId, out T saveObject) where T : SlotSaveObject
        {
            saveObject = GetSlotSaveObject<T>(slotId);

            if (saveObject == null)
            {
                SmDebugLogger.Log(SaveManagerErrorCode.NoSlotSaveObjectOfType.GetErrorMessageFormat(nameof(T)));
                return false;
            }

            return true;
        }
    }
}