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
using UnityEngine;

namespace CarterGames.Shared.SaveManager.Editor
{
    /// <summary>
    /// A copy of the Json data for each entry stored on the server.
    /// </summary>
    [Serializable]
    public sealed class VersionPacketSuccess
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private string date;
        [SerializeField] private string version;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The release date for the entry.
        /// </summary>
        public string Date
        {
            get => date;
            set => date = value;
        }
        
        
        /// <summary>
        /// The version for the entry.
        /// </summary>
        public string Version
        {
            get => version;
            set => version = value;
        }        
        

        /// <summary>
        /// The version number for the entry.
        /// </summary>
        public Version VersionNumber => new Version(Version);
    }
}