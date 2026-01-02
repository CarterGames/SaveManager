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

namespace CarterGames.Assets.SaveManager.Backups
{
    /// <summary>
    /// Stores the relevant data for a backup.
    /// </summary>
    [Serializable]
    public class SaveBackupData
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private int iteration;
        [SerializeField] private string json;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The iteration the backup is.
        /// </summary>
        public int Iteration => iteration;
        
        
        /// <summary>
        /// The JSON for the save data stored in the backup.
        /// </summary>
        public string Json => json;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Makes a new backup data.
        /// </summary>
        /// <param name="index">The iteration/index for the backup.</param>
        /// <param name="data">The data to store.</param>
        public SaveBackupData(int index, string data)
        {
            iteration = index;
            json = data;
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Bumps the iteration for this backup by one.
        /// </summary>
        public void BumpIteration()
        {
            iteration++;
        }
    }
}