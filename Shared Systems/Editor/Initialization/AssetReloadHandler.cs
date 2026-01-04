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
            var reloadClasses = AssemblyHelper.GetClassesOfType<IAssetEditorReload>();

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