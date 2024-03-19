/*
 * Copyright (c) 2024 Carter Games
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
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