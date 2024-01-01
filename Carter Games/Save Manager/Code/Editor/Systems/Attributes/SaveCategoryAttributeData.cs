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