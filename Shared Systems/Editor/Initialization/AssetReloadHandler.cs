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

using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Callbacks;

namespace CarterGames.Shared.SaveManager.Editor
{
    /// <summary>
    /// Handles any reload listeners in the project for the asset.
    /// </summary>
    public static class AssetReloadHandler
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private static readonly string AssetReloadKey = $"{AssetVersionData.AssetName}_Session_EditorReload";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if the logic has run since the last editor trigger for it.
        /// </summary>
        public static bool HasProcessed
        {
            get => SessionState.GetBool(AssetReloadKey, false);
            private set => SessionState.SetBool(AssetReloadKey, value);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises when the reload has occured.
        /// </summary>
        public static readonly Evt Reloaded = new Evt();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Add subscription to the delay call when scripts reload.
        /// </summary>
        [DidReloadScripts]
        private static void FireReloadCalls()
        {
            HasProcessed = false;
            
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorApplication.delayCall -= CallListeners;
                EditorApplication.delayCall += CallListeners;
                return;
            }
            
            EditorApplication.delayCall -= CallListeners;
            EditorApplication.delayCall += CallListeners;
        }
        

        /// <summary>
        /// Updates all the listeners when called.
        /// </summary>
        private static async void CallListeners()
        {
            var reloadClasses = AssemblyHelper.GetClassesOfType<IAssetEditorReload>(true);

            foreach (var init in reloadClasses)
            {
                init.OnEditorReloaded();
                await Task.Yield();
            }

            OnReloadProcessed();
        }

        
        private static void OnReloadProcessed()
        {
            Reloaded.Raise();
            HasProcessed = true;
        }
    }
}