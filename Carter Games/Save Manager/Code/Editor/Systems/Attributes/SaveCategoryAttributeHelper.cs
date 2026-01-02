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
using System.Reflection;
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Editor;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// A helper class to get the save objects in their categories via reflection.
    /// </summary>
    public static class SaveCategoryAttributeHelper
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string SaveObjectAndCategoryName = "CarterGames.Assets.SaveManager.SaveObject+SaveCategoryAttribute";
        private const string SaveCategoryIsExpandedFormat = "CarterGames.Assets.SaveManager.Category.{0}.IsExpanded";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Handles getting all the save objects with categories and putting those without into their own category.
        /// </summary>
        /// <returns>The data ready for use.</returns>
        private static IEnumerable<SaveCategoryAttributeData> GetObjectsWithCategory(IEnumerable<SaveObject> saveObjects)
        {
            var data = new List<SaveCategoryAttributeData>();
            
            foreach (var saveObj in saveObjects)
            {
                var attributes = saveObj.GetType().GetCustomAttributes();
            
                if (!attributes.Any(t => t.ToString().Equals(SaveObjectAndCategoryName)))
                {
                    data.Add(new SaveCategoryAttributeData(saveObj));
                    continue;
                }
            
                var category = attributes.First(t => t.ToString().Equals(SaveObjectAndCategoryName));
            
                var categoryName = category.GetType()
                    .GetField("Category", BindingFlags.Public | BindingFlags.Instance)?.GetValue(category).ToString();
            
                var order = int.Parse(category.GetType()
                    .GetField("OrderInCategory", BindingFlags.Public | BindingFlags.Instance)?.GetValue(category).ToString() ?? string.Empty);
                
                data.Add(new SaveCategoryAttributeData(categoryName, order, saveObj));
            }
            
            return data;
        }


        /// <summary>
        /// Gets all the save objects in the category named.
        /// </summary>
        /// <param name="categoryName">The category to look for.</param>
        /// <returns>A list of save objects in that category ordered correctly.</returns>
        public static List<SaveObject> GetObjectsInCategory(IEnumerable<SaveObject> saveObjects, string categoryName)
        {
            return GetObjectsWithCategory(saveObjects)
                .Where(t => t.CategoryName.Equals(categoryName))
                .OrderByDescending(t => t.OrderInCategory)
                .Select(t => t.SaveObject)
                .ToList();
        }
        
        
        /// <summary>
        /// Gets a list of all the defined save categories.
        /// </summary>
        /// <returns>The categories currently defined.</returns>
        public static List<string> GetCategoryNames(IEnumerable<SaveObject> saveObjects)
        {
            return GetObjectsWithCategory(saveObjects)
                .OrderBy(t => t.CategoryName)
                .Select(t => t.CategoryName)
                .Distinct()
                .ToList();
        }


        public static bool IsCategoryExpanded(string categoryName)
        {
            return (bool) PerUserSettingsEditor.GetOrCreateValue<bool>(
                string.Format(SaveCategoryIsExpandedFormat, categoryName), PerUserSettingType.PlayerPref, false);
        }


        public static void SetIsCategoryExpanded(string categoryName, bool value)
        {
            PerUserSettingsEditor.SetValue<bool>(string.Format(SaveCategoryIsExpandedFormat, categoryName),
                PerUserSettingType.PlayerPref, value);
        }
    }
}