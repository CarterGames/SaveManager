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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles finding assets in the project in editor space and creating/referencing/caching them for use.
    /// </summary>
    public static class FileEditorUtil
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// The name of the asset, used in path checks.
        /// </summary>
        public const string AssetName = "Save Manager";
        
        
        /// <summary>
        /// The path to a script in the asset to verify the asset base path.
        /// </summary>
        private static readonly string BasePathScriptPath = $"/Carter Games/{AssetName}/Code/Editor/Utility/{BasePathScriptName}.cs";
        
        
        /// <summary>
        /// The base path check script name.
        /// </summary>
        private const string BasePathScriptName = "UtilEditor";
        
        
        /// <summary>
        /// The base path cache.
        /// </summary>
        private static string basePath = "";

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The base path for the asset code.
        /// </summary>
        public static string AssetBasePath
        {
            get
            {
                if (basePath.Length > 0) return basePath;
                basePath = GetBaseAssetPath();
                return basePath;
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Getter Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the base path of the asset code, will break if the code is split up by the user.
        /// </summary>
        /// <returns>The base path found.</returns>
        private static string GetBaseAssetPath()
        {
            string path = string.Empty;
            var containsChecks = new List<string> { AssetName, $"/{BasePathScriptName}.cs" };
            
            foreach (var scriptFound in AssetDatabase.FindAssets($"t:Script {nameof(UtilEditor)}"))
            {
                path = AssetDatabase.GUIDToAssetPath(scriptFound);

                foreach (var check in containsChecks)
                {
                    if (!path.Contains(check)) goto SkipAndLoop;
                }
                
                path = AssetDatabase.GUIDToAssetPath(scriptFound);
                path = path.Replace(BasePathScriptPath, "");
                
                return path;
                
                // Skips the return as the path contained an invalid element for the asset...
                SkipAndLoop: ;
            }

            return path;
        }
        
        
        /// <summary>
        /// Gets a script file in the asset.
        /// </summary>
        /// <param name="assetPath">The path to the script.</param>
        /// <param name="pathContains">Parts of a string the path should contain.</param>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The found file as an object if found successfully.</returns>
        public static object GetScriptInAsset<T>(string assetPath, params string[] pathContains)
        {
            string path = string.Empty;
                
            foreach (var scriptFound in AssetDatabase.FindAssets($"t:Script {nameof(T)}"))
            {
                path = AssetDatabase.GUIDToAssetPath(scriptFound);

                foreach (var containCheck in pathContains)
                {
                    if (!path.Contains(containCheck)) goto Loop;
                }
                
                path = AssetDatabase.GUIDToAssetPath(scriptFound);
                return AssetDatabase.LoadAssetAtPath(path, typeof(T));
                Loop: ;
            }

            return null;
        }
        
        
        /// <summary>
        /// Gets a asset file in the asset.
        /// </summary>
        /// <param name="filter">The filter to check.</param>
        /// <param name="pathContains">Parts of a string the path should contain.</param>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The found file as an object if found successfully.</returns>
        public static object GetAssetInstance<T>(string filter, params string[] pathContains)
        {
            string path = string.Empty;
            
            foreach (var assetFound in AssetDatabase.FindAssets(filter, null))
            {
                path = AssetDatabase.GUIDToAssetPath(assetFound);

                foreach (var containCheck in pathContains)
                {
                    if (!path.Contains(containCheck)) goto Loop;
                }
                
                path = AssetDatabase.GUIDToAssetPath(assetFound);
                return AssetDatabase.LoadAssetAtPath(path, typeof(T));
                Loop: ;
            }

            return null;
        }
        
        
        /// <summary>
        /// Gets a asset file in the asset.
        /// </summary>
        /// <param name="filter">The filter to check.</param>
        /// <param name="assetPath">The path to check.</param>
        /// <param name="pathContains">Parts of a string the path should contain.</param>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The found file as an object if found successfully.</returns>
        public static object GetAssetInstance<T>(string filter, string assetPath, params string[] pathContains)
        {
            if (AssetDatabase.AssetPathToGUID(assetPath).Length > 0)
            {
                return AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
            }
            
            string path = string.Empty;
            
            foreach (var assetFound in AssetDatabase.FindAssets(filter, null))
            {
                path = AssetDatabase.GUIDToAssetPath(assetFound);

                foreach (var containCheck in pathContains)
                {
                    if (!path.Contains(containCheck)) goto Loop;
                }
                
                path = AssetDatabase.GUIDToAssetPath(assetFound);
                return AssetDatabase.LoadAssetAtPath(path, typeof(T));
                Loop: ;
            }

            return null;
        }
        
        
        /// <summary>
        /// Does the traditional get or assign cache method but with the new get asset instance variant.
        /// </summary>
        /// <param name="cache">The cache to update.</param>
        /// <param name="filter">The filter to search for.</param>
        /// <param name="pathContains">The strings to match in the path for the asset.</param>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>The updated cache.</returns>
        public static T GetOrAssignCache<T>(ref T cache, string filter, params string[] pathContains)
        {
            if (cache != null) return cache;
            cache = (T) GetAssetInstance<T>(filter, pathContains);
            return cache;
        }
        

        /// <summary>
        /// Creates a scriptable object or assigns the cache to an existing instance if one is found.
        /// </summary>
        /// <param name="cache">The cache to check.</param>
        /// <param name="filter">The filter to use.</param>
        /// <param name="path">The path to create the asset to if not found.</param>
        /// <param name="pathContains">Any string that should be in the path to make sure its the right asset.</param>
        /// <typeparam name="T">The type to check for.</typeparam>
        /// <returns>The found or created asset.</returns>
        public static T CreateSoGetOrAssignAssetCache<T>(ref T cache, string filter, string path, params string[] pathContains) where T : ScriptableObject
        {
            if (cache != null) return cache;

            cache = (T)GetAssetInstance<T>(filter, path, pathContains);

            if (cache == null)
            {
                cache = CreateScriptableObject<T>(path);
            }
            
            AssetIndexHandler.UpdateIndex();

            return cache;
        }
        
        
        /// <summary>
        /// Creates, gets or assigns a serialized object reference.
        /// </summary>
        /// <param name="cache">The cache to assign to.</param>
        /// <param name="reference">The reference to set from.,</param>
        /// <typeparam name="T">The type to reference.</typeparam>
        /// <returns>The updated cache.</returns>
        public static SerializedObject CreateGetOrAssignSerializedObjectCache<T>(ref SerializedObject cache, T reference)
        {
            if (cache != null && cache.targetObject != null) return cache;
            cache = new SerializedObject(reference as Object);
            return cache;
        }


        /// <summary>
        /// Creates a scriptable object of the type entered when called.
        /// </summary>
        /// <param name="path">The path to create the new asset at.</param>
        /// <typeparam name="T">The type to make.</typeparam>
        /// <returns>The newly created asset.</returns>
        private static T CreateScriptableObject<T>(string path) where T : ScriptableObject
        {
            var instance = ScriptableObject.CreateInstance(typeof(T));

            CreateToDirectory(path);

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.Refresh();

            return (T)instance;
        }
        
        
        /// <summary>
        /// Creates all the folders to a path if they don't exist already.
        /// </summary>
        /// <param name="path">The path to create to.</param>
        public static void CreateToDirectory(string path)
        {
            var currentPath = string.Empty;
            var split = path.Split('/');

            for (var i = 0; i < path.Split('/').Length; i++)
            {
                var element = path.Split('/')[i];
                currentPath += element + "/";

                if (i.Equals(split.Length - 1))continue;
                if (Directory.Exists(currentPath))continue;
                
                Directory.CreateDirectory(currentPath);
            }
        }
        
        
        /// <summary>
        /// Deletes a directory and any assets within when called.
        /// </summary>
        /// <param name="path">The path to delete.</param>
        public static void DeleteDirectoryAndContents(string path)
        {
            foreach (var file in Directory.GetFiles(path).ToList())
            {
                AssetDatabase.DeleteAsset(file);
            }

            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}