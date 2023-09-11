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

using UnityEditor;
using UnityEngine.Windows;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the removal of any legacy editor settings asset for this asset.
    /// </summary>
    /// <remarks>
    /// It was moved to editor prefs as of 2.0.13 to aid with version control on group projects with the asset.
    /// </remarks>
    public sealed class LegacyEditorSettingsRemover : AssetPostprocessor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static readonly string OldEditorSettingsPath =
            $"{FileEditorUtil.AssetBasePath}/Carter Games/{FileEditorUtil.AssetName}/Data/Editor Settings.asset";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            Mitigate();
        }
        
        
        /// <summary>
        /// Runs the mitigation for the editor settings to be deleted.
        /// </summary>
        private static void Mitigate()
        {
            if (!File.Exists(OldEditorSettingsPath)) return;
            
            // Remove the file as its not needed anymore as of (2.0.13)
            File.Delete(OldEditorSettingsPath);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}