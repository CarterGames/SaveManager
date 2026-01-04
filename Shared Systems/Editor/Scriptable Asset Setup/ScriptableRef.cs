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

namespace CarterGames.Shared.SaveManager.Editor
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
        private static readonly string PathResources = $"{AssetVersionData.AssetName}/Resources/";
        private static readonly string PathData = $"{AssetVersionData.AssetName}/Data/";

        public static readonly string FullPathResources = $"{Root}{PathResources}";
        public static readonly string FullPathData = $"{Root}{PathData}";
        
        
        private static Dictionary<Type, IScriptableAssetDef<SmDataAsset>> cacheLookup;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Handles a lookup of all the assets in the project.
        /// </summary>
        private static Dictionary<Type, IScriptableAssetDef<SmDataAsset>> AssetLookup
        {
            get
            {
                if (cacheLookup != null)
                {
                    if (cacheLookup.Count > 0) return cacheLookup;
                }
                
                cacheLookup = new Dictionary<Type, IScriptableAssetDef<SmDataAsset>>();

                foreach (var elly in AssemblyHelper.GetClassesOfType<IScriptableAssetDef<SmDataAsset>>())
                {
                    cacheLookup.Add(elly.AssetType, elly);
                }
                
                return cacheLookup;
            }   
        }
        
        
        // Helper Properties
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        // Assets initialized Check
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if all the assets needed for the asset to function are in the project at the expected paths.
        /// </summary>
        public static bool HasAllAssets()
        {
            return AssetLookup.All(t => HasAsset(t.Value));
        }
        

        /// <summary>
        /// Gets a scriptable asset definition.
        /// </summary>
        /// <typeparam name="T">The type of the scriptable asset.</typeparam>
        /// <returns>The asset definition found.</returns>
        public static IScriptableAssetDef<T> GetAssetDef<T>() where T : SmDataAsset
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
        public static bool TryGetAssetDef<T>(out IScriptableAssetDef<T> asset) where T : SmDataAsset
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
        private static bool HasAsset<T>(IScriptableAssetDef<T> def) where T : SmDataAsset
        {
            return AssetDatabaseHelper.FileIsInProject<T>(def.DataAssetPath);
        }
        
        
        /// <summary>
        /// Tries to create the asset requested.
        /// </summary>
        /// <param name="def">The definition to use.</param>
        /// <param name="cache">The cache for the definition.</param>
        /// <typeparam name="T">The type of the scriptable asset.</typeparam>
        public static void TryCreateAsset<T>(IScriptableAssetDef<T> def, ref T cache) where T : SmDataAsset
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
        public static T GetOrCreateAsset<T>(IScriptableAssetDef<T> def, ref T cache) where T : SmDataAsset
        {
            return FileEditorUtil.CreateSoGetOrAssignAssetCache(
                ref cache, 
                def.DataAssetFilter, 
                def.DataAssetPath, 
                AssetVersionData.AssetName, $"{PathData}{def.DataAssetFileName}");
        }

        
        /// <summary>
        /// Gets the existing reference or creates on if its not in the project currently.
        /// </summary>
        /// <param name="def">The definition to use.</param>
        /// <param name="objCache">The cache for the definition.</param>
        /// <typeparam name="T">The type of the scriptable asset.</typeparam>
        /// <returns>The object reference</returns>
        public static SerializedObject GetOrCreateAssetObject<T>(IScriptableAssetDef<T> def, ref SerializedObject objCache) where T : SmDataAsset
        {
            return FileEditorUtil.CreateGetOrAssignSerializedObjectCache(ref objCache, def.AssetRef);
        }
    }
}