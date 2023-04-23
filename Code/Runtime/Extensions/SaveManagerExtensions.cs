/*
 * Copyright (c) 2018-Present Carter Games
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

using UnityEngine;

namespace CarterGames.Assets.SaveManager.Extensions
{
    /// <summary>
    /// A class holding any extensions methods needed for the asset at runtime.
    /// </summary>
    public static class SaveManagerExtensions
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
        |   Extension Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Converts the string entered into a valid path, parsing any %...%'s into their respective values.
        /// </summary>
        /// <param name="path">The string to parse.</param>
        /// <returns>The parsed path.</returns>
        public static string ParseSavePath(this string path)
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