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
using System.Linq;
using UnityEngine;

namespace CarterGames.Shared.SaveManager
{
    /// <summary>
    /// Handles accessing the scriptable object data assets for this asset.
    /// </summary>
    public static class SmAssetAccessor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
     
        private const string IndexPath = "[Save Manager] Asset Index";
        
        // A cache of all the assets found...
        private static SmDataAssetIndex indexCache;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets all the assets from the build versions asset...
        /// </summary>
        private static SmDataAssetIndex Index
        {
            get
            {
                if (indexCache != null) return indexCache;
                indexCache = (SmDataAssetIndex) Resources.Load(IndexPath, typeof(SmDataAssetIndex));
                return indexCache;
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets the Save Manager Asset requested.
        /// </summary>
        /// <typeparam name="T">The save manager asset to get.</typeparam>
        /// <returns>The asset if it exists.</returns>
        public static T GetAsset<T>() where T : SmDataAsset
        {
            if (Index.Lookup.ContainsKey(typeof(T).ToString()))
            {
                return (T)Index.Lookup[typeof(T).ToString()][0];
            }

            return null;
        }
        
        
        /// <summary>
        /// Gets the Save Manager Asset requested.
        /// </summary>
        /// <typeparam name="T">The save manager asset to get.</typeparam>
        /// <returns>The asset if it exists.</returns>
        public static List<T> GetAssets<T>() where T : SmDataAsset
        {
            if (Index.Lookup.ContainsKey(typeof(T).ToString()))
            {
                return Index.Lookup[typeof(T).ToString()].Cast<T>().ToList();
            }

            return null;
        }
    }
}