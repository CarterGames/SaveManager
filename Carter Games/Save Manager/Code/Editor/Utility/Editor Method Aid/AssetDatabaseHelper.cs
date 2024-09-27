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
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class AssetDatabaseHelper
    {
        /// <summary>
        /// Gets if asset database can find an asset at the defined path.
        /// </summary>
        /// <param name="path">The path top find.</param>
        /// <typeparam name="T">The type to try and get.</typeparam>
        /// <returns>If the asset exists in the asset database.</returns>
        public static bool FileIsInProject<T>(string path) where T : SaveManagerAsset
        {
            if (string.IsNullOrEmpty(path)) return false;
            return AssetDatabase.LoadAssetAtPath(path, typeof(T)) != null;
        }
        
        
        
        /// <summary>
        /// Gets the string paths for any asset of the type not in the expected location.
        /// </summary>
        /// <param name="expectedPath">The path the asset should be at.</param>
        /// <typeparam name="T">The type to check for.</typeparam>
        /// <returns>The paths for any asset not where it should be.</returns>
        public static IEnumerable<string> GetAssetPathNotAtPath<T>(string expectedPath) where T : SaveManagerAsset
        {
            if (TryGetPathsToAssetsNotAtPath<T>(expectedPath, out var result))
            {
                return result;
            }

            return null;
        }
        
        
        /// <summary>
        /// Returns if an instance of the asset type is not where it should be.
        /// </summary>
        /// <param name="expectedPath">The path it should be at.</param>
        /// <typeparam name="T">The type to try and get.</typeparam>
        /// <returns>If the asset correctly exists in the asset database.</returns>
        public static bool TypeExistsElsewhere<T>(string expectedPath) where T : SaveManagerAsset
        {
            return TryGetTypeNotAtPath<T>(expectedPath, out _);
        }


        /// <summary>
        /// A helper method to get assets of a type not at the expected path.
        /// </summary>
        /// <param name="expectedPath">The path the asset should be at.</param>
        /// <param name="result">The result of the operation.</param>
        /// <typeparam name="T">The type to try and get.</typeparam>
        /// <returns>If it was successful at finding any assets at the wrong path.</returns>
        private static bool TryGetTypeNotAtPath<T>(string expectedPath, out IEnumerable<T> result) where T : SaveManagerAsset
        {
            if (!TryGetPathsToAssetsNotAtPath<T>(expectedPath, out var paths))
            {
                result = null;
                return false;
            }
            
            result = paths.Select(t => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(t)));
            return true;
        }
        
        
        /// <summary>
        /// A helper method to get paths of any asset of a type not at the expected path.
        /// </summary>
        /// <param name="expectedPath">The path the asset should be at.</param>
        /// <param name="result">The result of the operation.</param>
        /// <typeparam name="T">The type to try and get.</typeparam>
        /// <returns>If it was successful at finding any assets at the wrong path.</returns>
        private static bool TryGetPathsToAssetsNotAtPath<T>(string expectedPath, out IEnumerable<string> result) where T : SaveManagerAsset
        {
            result = null;
            
            if (string.IsNullOrEmpty(expectedPath)) return false;
            var assets = AssetDatabase.FindAssets($"t:{typeof(T).FullName}");

            if (assets.Length <= 1)
            {
                if (AssetDatabase.GUIDToAssetPath(assets.First()) == expectedPath) return false;
                
                result = new string[1]
                {
                    AssetDatabase.GUIDToAssetPath(assets.First()) 
                };
                
                return true;
            }

            result = assets.Where(t => AssetDatabase.GUIDToAssetPath(t) != expectedPath).Select(AssetDatabase.GUIDToAssetPath);
            return true;
        }
    }
}