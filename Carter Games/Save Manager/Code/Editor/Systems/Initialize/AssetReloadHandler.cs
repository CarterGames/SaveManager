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

using System.Linq;
using System.Threading.Tasks;
using CarterGames.Common;
using UnityEditor;
using UnityEditor.Callbacks;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles any reload listeners in the project for the asset.
    /// </summary>
    public static class AssetReloadHandler
    {
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
            var reloadClasses = AssemblyHelper.GetClassesOfType<IAssetEditorReload>().ToArray();
            
            if (reloadClasses.Length > 0)
            {
                foreach (var init in reloadClasses)
                {
                    init.OnEditorReloaded();
                    await Task.Yield();
                }
            }
            
            Reloaded.Raise();
        }
    }
}