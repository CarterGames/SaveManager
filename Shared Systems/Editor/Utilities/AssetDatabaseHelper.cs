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
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CarterGames.Shared.SaveManager.Editor
{
    public static class AssetDatabaseHelper
    {
        /// <summary>
        /// Gets if asset database can find an asset at the defined path.
        /// </summary>
        /// <param name="path">The path top find.</param>
        /// <typeparam name="T">The type to try and get.</typeparam>
        /// <returns>If the asset exists in the asset database.</returns>
        public static bool FileIsInProject<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.Log($"Unable to find asset at the path: {path}");
                return false;
            }
            
            return AssetDatabase.LoadAssetAtPath(path, typeof(T)) != null;
        }


        /// <summary>
        /// Tries to get the path to the entered script.
        /// </summary>
        /// <param name="path">The path found.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>If it was successful.</returns>
        public static bool TryGetScriptPath<T>(out string path)
        {
            return TryGetScriptPath(typeof(T), out path);
        }
        
        
        /// <summary>
        /// Tries to get the path to the entered script.
        /// </summary>
        /// <param name="type">The type to find.</param>
        /// <param name="path">The path found.</param>
        /// <returns>If it was successful.</returns>
        public static bool TryGetScriptPath(Type type, out string path)
        {
            path = string.Empty;

            var foundClass = AssetDatabase.FindAssets($"t:Script {type.Name}");
            if (foundClass == null) return false;
            if (foundClass.Length <= 0) return false;
            
            path = AssetDatabase.GUIDToAssetPath(foundClass[0]);
            
            return !string.IsNullOrEmpty(path);
        }
        
        
        /// <summary>
        /// Gets all the instances of a class in the project.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>All the instances found.</returns>
        public static T[] GetAllInstancesInProject<T>() where T : Object
        {
            var assets = AssetDatabase.FindAssets($"t:{typeof(T)}");
            
            if (assets == null) return Array.Empty<T>();

            var array = new T[assets.Length];

            for (var i = 0; i < array.Length; i++)
            {
                array[i] = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[i]));
            }

            return array;
        }
        
        
        /// <summary>
        /// Gets the string paths for any asset of the type not in the expected location.
        /// </summary>
        /// <param name="expectedPath">The path the asset should be at.</param>
        /// <typeparam name="T">The type to check for.</typeparam>
        /// <returns>The paths for any asset not where it should be.</returns>
        public static IEnumerable<string> GetAssetPathNotAtPath<T>(string expectedPath)
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
        public static bool TypeExistsElsewhere<T>(string expectedPath) where T : Object
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
        private static bool TryGetTypeNotAtPath<T>(string expectedPath, out IEnumerable<T> result) where T : Object
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
        private static bool TryGetPathsToAssetsNotAtPath<T>(string expectedPath, out IEnumerable<string> result)
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