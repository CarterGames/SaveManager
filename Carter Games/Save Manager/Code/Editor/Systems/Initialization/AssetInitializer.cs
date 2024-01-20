/*
 * Copyright (c) 2018-Present Carter Games
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

using System.IO;
using System.Reflection;
using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the auto setup of the save manager when the assets are changed in the project.
    /// </summary>
    public class AssetInitializer : AssetPostprocessor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Runs after assets have imported / script reload etc at a safe time to edit assets.
        /// </summary>
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            TryInitialize();
        }


        /// <summary>
        /// Initializes the save data on project load
        /// </summary>
        [InitializeOnLoadMethod]
        private static void TryInitSave()
        {
            if (SessionState.GetBool("HasLoaded", false)) return;
            DelayUpdate();
        }


        /// <summary>
        /// Listen for a delayed update to ensure the editor is all loaded before loading data.
        /// </summary>
        private static void DelayUpdate()
        {
            EditorApplication.delayCall -= LoadOnProjectOpen;
            EditorApplication.delayCall += LoadOnProjectOpen;
        }


        /// <summary>
        /// Loads the latest data into the save system for use.
        /// </summary>
        private static void LoadOnProjectOpen()
        {
            EditorApplication.delayCall -= LoadOnProjectOpen;

            // Initializes the Save Manager and loads the data for use.
            typeof(SaveManager).GetMethod("Initialize", BindingFlags.NonPublic | BindingFlags.Static)?.Invoke(null, null);
            
            SessionState.SetBool("HasLoaded", true);
        }
        
        
        /// <summary>
        /// Creates the scriptable objects for the asset if they don't exist yet.
        /// </summary>
        private static void TryInitialize()
        {
            LegacyIndexRemovalTool.TryRemoveOldIndex();
            
            if (UtilEditor.HasInitialized) return;
            UtilEditor.Initialize();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
