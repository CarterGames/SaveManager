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

using System.Linq;
using System.Threading.Tasks;
using UnityEditor;

namespace CarterGames.Shared.SaveManager.Editor
{
    /// <summary>
    /// Handles any initial listeners in the project for the asset.
    /// </summary>
    public static class AssetInitializer
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        // The key for if the asset has been initialized.
        private static readonly string AssetInitializeKey = $"{AssetVersionData.AssetName}_Session_EditorInitialize";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if the asset is initialized or not. 
        /// </summary>
        public static bool IsInitialized
        {
            get => SessionState.GetBool(AssetInitializeKey, false);
            private set => SessionState.SetBool(AssetInitializeKey, value);
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Is raised when the asset is initialized.
        /// </summary>
        public static readonly Evt Initialized = new Evt();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initializes the editor logic for the asset when called.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void TryInitialize()
        {
            if (IsInitialized) return;
            InitializeEditorClasses();
        }


        /// <summary>
        /// Runs through all interfaces for initializing the editor asset logic and runs each in the defined order.
        /// </summary>
        private static async void InitializeEditorClasses()
        {
            var initClasses = AssemblyHelper.GetClassesOfType<IAssetEditorInitialize>(true);

            foreach (var init in initClasses.OrderBy(t => t.InitializeOrder))
            {
                init.OnEditorInitialized();
                await Task.Yield();
            }

            OnAllClassesInitialized();
        }
        


        /// <summary>
        /// Runs any post initialize logic to complete the process.
        /// </summary>
        private static void OnAllClassesInitialized()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            IsInitialized = true;
            Initialized.Raise();
        }
    }
}