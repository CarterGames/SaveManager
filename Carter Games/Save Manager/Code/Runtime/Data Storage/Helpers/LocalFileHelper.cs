using UnityEngine;

namespace CarterGames.Assets.SaveManager.Helpers
{
    public static class LocalFileHelper
    {
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