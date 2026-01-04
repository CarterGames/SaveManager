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

using UnityEngine;

namespace CarterGames.Assets.SaveManager.Helpers
{
    /// <summary>
    /// A helper class for local file saving.
    /// </summary>
    public static class LocalFileHelper
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The "Application.persistentDataPath" identifier.
        /// </summary>
        private const string PersistentDataPathIdentifier = "%Application.persistentDataPath%";

        
        /// <summary>
        /// The "Application.dataPath" identifier.
        /// </summary>
        private const string DataPathIdentifier = "%Application.dataPath%";
        
        
        /// <summary>
        /// The "Product Name" identifier.
        /// </summary>
        private const string ProductNameIdentifier = "%productName%";
        
        
        /// <summary>
        /// The "Company Name" identifier.
        /// </summary>
        private const string CompanyNameIdentifier = "%companyName%";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Parses the save path to something usable.
        /// </summary>
        /// <param name="path">The path to parse.</param>
        /// <returns>The parsed path.</returns>
        public static string ParseSavePath(string path)
        {
            var newPath = path;
            
            if (path.Contains(PersistentDataPathIdentifier))
            {
                newPath = newPath.Replace(PersistentDataPathIdentifier, Application.persistentDataPath);
            }
            
            if (path.Contains(DataPathIdentifier))
            {
                newPath = newPath.Replace(DataPathIdentifier, Application.dataPath);
            }
            
            if (path.Contains(ProductNameIdentifier))
            {
                newPath = newPath.Replace(ProductNameIdentifier, Application.productName);
            }
            
            if (path.Contains(CompanyNameIdentifier))
            {
                newPath = newPath.Replace(CompanyNameIdentifier, Application.companyName);
            }

            return newPath;
        }
    }
}