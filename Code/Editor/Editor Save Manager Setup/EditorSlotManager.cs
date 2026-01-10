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
using CarterGames.Assets.SaveManager.Slots;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// A editor specific API access for save slots.
    /// </summary>
    public static class EditorSlotManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The total slots defined.
        /// </summary>
        public static int TotalSlots => SaveSlotManager.TotalSlotsInUse;
        
        
        /// <summary>
        /// A collection of all the slots to refer to, but not edit.
        /// </summary>
        public static IReadOnlyDictionary<int, SaveSlot> AllSlotsData => SaveSlotManager.AllSlots;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Adds a new slot from GUI when called.
        /// </summary>
        public static void AddNewSlot()
        {
            if (SaveSlotManager.TryCreateSlot(out var newSlot))
            {
                EditorSaveObjectController.AddEditorsForSaveSlot(newSlot);
            }
        }

        
        /// <summary>
        /// Deletes a slot.
        /// </summary>
        /// <param name="saveSlot">The slot to delete.</param>
        public static void DeleteSlot(SaveSlot saveSlot)
        {
            SaveSlotManager.DeleteSlot(saveSlot.SlotId);
        }
    }
}