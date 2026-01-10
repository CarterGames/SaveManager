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
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A base class for a save value.
    /// </summary>
    [Serializable]
    public abstract class SaveValueBase
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The key for the save value.
        /// </summary>
        [HideInInspector] public string key;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The value of the save value as an object.
        /// </summary>
        public abstract object ValueObject { get; set; }
        
        
        /// <summary>
        /// Gets the value type stored in the save value.
        /// </summary>
        public abstract Type ValueType { get; }
        
        
        /// <summary>
        /// The default value of the save value as an object.
        /// </summary>
        protected abstract object DefaultValueObject { get; set; }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Reset the value on call.
        /// </summary>
        public abstract void ResetValue(bool useDefault = true);
        
        
        /// <summary>
        /// Assign the save value from json
        /// </summary>
        /// <param name="value">The json value to assign from.</param>
        public abstract void AssignFromJson(JToken value);
    }
}