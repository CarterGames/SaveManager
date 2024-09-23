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

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles any logic for generating/updating the scriptable objects for the asset where needed.
    /// </summary>
    public sealed class ScriptableObjectInitialize : IAssetEditorInitialize, IAssetEditorReload
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   IAssetEditorInitialize
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Defines the order that this initializer run at.
        /// </summary>
        public int InitializeOrder => -1;
        

        /// <summary>
        /// Runs when the asset initialize flow is used.
        /// </summary>
        public void OnEditorInitialized()
        {
            if (ScriptableRef.HasAllAssets()) return;
            ScriptableRef.TryCreateAssets();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   IAssetEditorReload
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Runs when the asset reload flow is used.
        /// </summary>
        public void OnEditorReloaded()
        {
            if (ScriptableRef.HasAllAssets()) return;
            ScriptableRef.TryCreateAssets();
        }
    }
}