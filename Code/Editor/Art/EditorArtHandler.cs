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
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the art for the editor setup. Without needing exact references.
    /// </summary>
    public static class EditorArtHandler
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static readonly IReadOnlyDictionary<string, string> DefinesLookup = new Dictionary<string, string>()
        {
            { SaveManagerConstants.WarningIcon, "Art/Editor/General/T_Warning_Filled_Icon.png" },
            { SaveManagerConstants.BookIcon, "Art/Editor/Icons/Inspectors/T_SaveManager_BookIcon.png" },
            { SaveManagerConstants.CogIcon, "Art/Editor/Icons/Inspectors/T_SaveManager_CogIcon.png" },
            { SaveManagerConstants.DataIcon, "Art/Editor/Icons/Inspectors/T_SaveManager_DataIcon.png" },
            { SaveManagerConstants.WindowIcon, "Art/Editor/Icons/Windows/T_SaveManager_SaveWhite.png" },
        };
        
        private static readonly Dictionary<string, Texture2D> CacheLookup = new Dictionary<string, Texture2D>();

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the asset path for any art asset for the asset from this script.
        /// </summary>
        /// <param name="pathEnd"></param>
        /// <returns></returns>
        private static string GetPathToAsset(string pathEnd)
        {
            string basePath = Path.GetFullPath(Path.Combine(GetCurrentFileName(), $"../../../../{pathEnd}")).Replace(@"\", "/");;
            
            // Needed as otherwise the art gets a null ref in the package manager version.
            if (Directory.Exists("Assets/Carter Games/Save Manager"))
            {
                return "Assets/" + basePath.Split(new string[1] { "/Assets/" }, StringSplitOptions.None)[1];
            }
            else
            {
                return "Art/" + basePath.Split(new string[1] { "/Art/" }, StringSplitOptions.None)[1];
            }
        }
        
        
        /// <summary>
        /// Gets the file name for the location of this class.
        /// </summary>
        /// <param name="fileName">The path for this file.</param>
        /// <returns>String</returns>
        private static string GetCurrentFileName([System.Runtime.CompilerServices.CallerFilePath] string fileName = null)
        {
            return fileName;
        }


        /// <summary>
        /// Gets an art icon from its constant id.
        /// </summary>
        /// <param name="id">The id to get</param>
        /// <returns>Texture2D</returns>
        public static Texture2D GetIcon(string id)
        {
            if (CacheLookup.ContainsKey(id))
            {
                if (CacheLookup[id] == null)
                {
                    CacheLookup[id] = AssetDatabase.LoadAssetAtPath<Texture2D>(GetPathToAsset(DefinesLookup[id]));
                }
                    
                return CacheLookup[id];
            }
            
            if (!DefinesLookup.ContainsKey(id)) return null;
            CacheLookup.Add(id, AssetDatabase.LoadAssetAtPath<Texture2D>(GetPathToAsset(DefinesLookup[id])));
            return CacheLookup[id];
        }
    }
}