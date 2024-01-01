using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Handles getting all the save objects with categories and putting those without into their own category.
        /// </summary>
        /// <returns>The data ready for use.</returns>
        private static IEnumerable<SaveCategoryAttributeData> GetObjectsWithCategory()
        {
            var saveObjects = AssetAccessor.GetAsset<SaveData>().Data;
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
        public static List<SaveObject> GetObjectsInCategory(string categoryName)
        {
            return GetObjectsWithCategory()
                .Where(t => t.CategoryName.Equals(categoryName))
                .OrderByDescending(t => t.OrderInCategory)
                .Select(t => t.SaveObject)
                .ToList();
        }
        
        
        /// <summary>
        /// Gets a list of all the defined save categories.
        /// </summary>
        /// <returns>The categories currently defined.</returns>
        public static List<string> GetCategoryNames()
        {
            return GetObjectsWithCategory()
                .OrderBy(t => t.CategoryName)
                .Select(t => t.CategoryName)
                .Distinct()
                .ToList();
        }
    }
}