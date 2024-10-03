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
using System.Collections.Generic;
using System.Linq;
using CarterGames.Common;
using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles references to scriptable objects in the asset that need generating without user input etc.
    /// </summary>
    public static class ScriptableRef
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private static readonly string Root = $"Assets/Plugins/Carter Games/";
        private static readonly string PathResources = $"{AssetName}/Resources/";
        private static readonly string PathData = $"{AssetName}/Data/";

        public static readonly string FullPathResources = $"{Root}{PathResources}";
        public static readonly string FullPathData = $"{Root}{PathData}";
        
        private static Dictionary<Type, IScriptableAssetDef<SaveManagerAsset>> cacheLookup;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        // Helper Properties
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        
        /// <summary>
        /// Gets the asset name stored in the file util editor class.
        /// </summary>
        private static string AssetName => FileEditorUtil.AssetName;
        
        
        // Asset References
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        
        /// <summary>
        /// Handles a lookup of all the assets in the project.
        /// </summary>
        private static Dictionary<Type, IScriptableAssetDef<SaveManagerAsset>> AssetLookup
        {
            get
            {
                if (cacheLookup != null)
                {
                    if (cacheLookup.Count > 0) return cacheLookup;
                }
                
                cacheLookup = new Dictionary<Type, IScriptableAssetDef<SaveManagerAsset>>();

                foreach (var elly in AssemblyHelper.GetClassesOfType<IScriptableAssetDef<SaveManagerAsset>>())
                {
                    cacheLookup.Add(elly.AssetType, elly);
                }
                
                return cacheLookup;
            }   
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets if all the assets needed for the asset to function are in the project at the expected paths.
        /// </summary>
        public static bool HasAllAssets()
        {
            return AssetLookup.All(t => HasAsset(t.Value));
        }
        
        
        /// <summary>
        /// Tries to create any missing assets when called.
        /// </summary>
        public static void TryCreateAssets()
        {
            foreach (var entry in AssetLookup)
            {
                entry.Value.TryCreate();
            }
        }
        

        /// <summary>
        /// Gets a scriptable asset definition.
        /// </summary>
        /// <typeparam name="T">The type of the scriptable asset.</typeparam>
        /// <returns>The asset definition found.</returns>
        public static IScriptableAssetDef<T> GetAssetDef<T>() where T : SaveManagerAsset
        {
            if (AssetLookup.ContainsKey(typeof(T)))
            {
                return (IScriptableAssetDef<T>) AssetLookup[typeof(T)];
            }

            return null;
        }
        
        
        /// <summary>
        /// Tries to get a scriptable asset definition.
        /// </summary>
        /// <param name="asset">The asset found.</param>
        /// <typeparam name="T">The type of the scriptable asset.</typeparam>
        /// <returns>If it was successful or not.</returns>
        public static bool TryGetAssetDef<T>(out IScriptableAssetDef<T> asset) where T : SaveManagerAsset
        {
            asset = GetAssetDef<T>();
            return asset != null;
        }
        

        /// <summary>
        /// Gets if an asset if currently in the project.
        /// </summary>
        /// <param name="def">The definition to check</param>
        /// <typeparam name="T">The type of the scriptable asset.</typeparam>
        /// <returns>If the asset exists.</returns>
        private static bool HasAsset<T>(IScriptableAssetDef<T> def) where T : SaveManagerAsset
        {
            return AssetDatabaseHelper.FileIsInProject<T>(def.DataAssetPath);
        }
        
        
        /// <summary>
        /// Tries to create the asset requested.
        /// </summary>
        /// <param name="def">The definition to use.</param>
        /// <param name="cache">The cache for the definition.</param>
        /// <typeparam name="T">The type of the scriptable asset.</typeparam>
        public static void TryCreateAsset<T>(IScriptableAssetDef<T> def, ref T cache) where T : SaveManagerAsset
        {
            if (cache != null) return;
            GetOrCreateAsset(def, ref cache);
        }
        

        /// <summary>
        /// Gets the existing reference or creates on if its not in the project currently.
        /// </summary>
        /// <param name="def">The definition to use.</param>
        /// <param name="cache">The cache for the definition.</param>
        /// <typeparam name="T">The type of the scriptable asset.</typeparam>
        /// <returns>The asset reference.</returns>
        public static T GetOrCreateAsset<T>(IScriptableAssetDef<T> def, ref T cache) where T : SaveManagerAsset
        {
            return FileEditorUtil.CreateSoGetOrAssignAssetCache(
                ref cache, 
                def.DataAssetFilter, 
                def.DataAssetPath, 
                AssetName, $"{PathData}{def.DataAssetFileName}");
        }

        
        /// <summary>
        /// Gets the existing reference or creates on if its not in the project currently.
        /// </summary>
        /// <param name="def">The definition to use.</param>
        /// <param name="objCache">The cache for the definition.</param>
        /// <typeparam name="T">The type of the scriptable asset.</typeparam>
        /// <returns>The object reference</returns>
        public static SerializedObject GetOrCreateAssetObject<T>(IScriptableAssetDef<T> def, ref SerializedObject objCache) where T : SaveManagerAsset
        {
            return FileEditorUtil.CreateGetOrAssignSerializedObjectCache(ref objCache, def.AssetRef);
        }
    }
}