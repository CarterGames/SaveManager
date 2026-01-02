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
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Slots
{
    /// <summary>
    /// A container class for save slot objects.
    /// </summary>
    public sealed class SlotSaveObjectsContainer
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the save objects stored in the container.
        /// </summary>
        public IEnumerable<SaveObject> SlotSaveObjects { get; private set; }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public SlotSaveObjectsContainer()
        {
            var list = new List<SaveObject>();

            var saveObjectsTypes = SaveObjectController.SlotSaveObjectTypes;

            foreach (var soType in saveObjectsTypes)
            {
                list.Add((SaveObject)ScriptableObject.CreateInstance(soType));
            }
            
            SlotSaveObjects = list;
        }
    }
}