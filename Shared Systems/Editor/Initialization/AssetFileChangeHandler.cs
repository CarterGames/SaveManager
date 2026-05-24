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

namespace CarterGames.Shared.SaveManager.Editor
{
    public class AssetFileChangeHandler : AssetPostprocessor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private static readonly string AssetFileChangeKey = $"{AssetVersionData.AssetName}_Session_EditorFileChange";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if the logic has run since the last editor trigger for it.
        /// </summary>
        public static bool HasProcessed
        {
            get => SessionState.GetBool(AssetFileChangeKey, false);
            private set => SessionState.SetBool(AssetFileChangeKey, value);
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public static readonly Evt ChangesDetected = new Evt();
        
        
        public override int GetPostprocessOrder() => 100;


        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (importedAssets.Length <= 0 && deletedAssets.Length <= 0 && 
                movedAssets.Length <= 0 && movedFromAssetPaths.Length <= 0) return;

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
            if (HasProcessed) return;
            
            var fileChangeClasses = AssemblyHelper.GetClassesOfType<IAssetEditorFileChanges>(true);

            foreach (var init in fileChangeClasses)
            {
                init.OnEditorFileChanges();
                await Task.Yield();
            }

            ChangesDetected.Raise();
            HasProcessed = true;
        }
    }
}