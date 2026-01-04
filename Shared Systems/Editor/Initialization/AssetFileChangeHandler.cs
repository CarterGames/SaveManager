/*
 * Copyright (c) 2025 Carter Games
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
            
            var fileChangeClasses = AssemblyHelper.GetClassesOfType<IAssetEditorFileChanges>();

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