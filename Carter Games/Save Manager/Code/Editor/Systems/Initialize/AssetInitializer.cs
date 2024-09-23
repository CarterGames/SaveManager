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

namespace CarterGames.Assets.SaveManager.Editor
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
        private static readonly string AssetInitializeKey = $"{FileEditorUtil.AssetName}_Session_EditorInitialize";
        
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
            var initClasses = AssemblyHelper.GetClassesOfType<IAssetEditorInitialize>().ToArray();
            
            if (initClasses.Length > 0)
            {
                foreach (var init in initClasses.OrderBy(t => t.InitializeOrder))
                {
                    init.OnEditorInitialized();
                    await Task.Yield();
                }
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