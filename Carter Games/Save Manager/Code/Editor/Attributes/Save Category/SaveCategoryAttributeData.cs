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

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the data for a save category to be processed.
    /// </summary>
    [Serializable]
    public sealed class SaveCategoryAttributeData
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField, HideInInspector] private string categoryName;
        [SerializeField, HideInInspector] private int orderInCategory;
        [SerializeField] private SaveObject saveObject;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The name of the category.
        /// </summary>
        public string CategoryName => categoryName;
        
        
        /// <summary>
        /// The order it should be shown in the category.
        /// </summary>
        public int OrderInCategory => orderInCategory;
        
        
        /// <summary>
        /// The save object the category applies to.
        /// </summary>
        public SaveObject SaveObject => saveObject;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// A new attribute data for the entered save object.
        /// </summary>
        /// <param name="saveObject">The object to use.</param>
        public SaveCategoryAttributeData(SaveObject saveObject)
        {
            categoryName = "Uncategorized";
            orderInCategory = 0;
            this.saveObject = saveObject;
        }
        
        
        /// <summary>
        /// A new attribute data for the entered save object.
        /// </summary>
        /// <param name="categoryName">The category to sort into.</param>
        /// <param name="order">The order to apply for the object in the category.</param>
        /// <param name="saveObject">The object to use.</param>
        public SaveCategoryAttributeData(string categoryName, int order, SaveObject saveObject)
        {
            this.categoryName = categoryName;
            orderInCategory = order;
            this.saveObject = saveObject;
        }
    }
}