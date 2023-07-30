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

using System.Collections.Generic;
using CarterGames.Assets.SaveManager.Utility;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles the setup of the asset index for runtime references to scriptable objects used for the asset.
    /// </summary>
    public sealed class AssetIndexHandler : IPreprocessBuildWithReport
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private const string AssetFilter = "t:savemanagerasset";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   IPreprocessBuildWithReport Implementation
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The order this script is processed in, in this case its the default.
        /// </summary>
        public int callbackOrder => 0;
        
        
        /// <summary>
        /// Runs before a build is executed.
        /// </summary>
        /// <param name="report">The report about the build (I don't need it, but its a param for the method).</param>
        public void OnPreprocessBuild(BuildReport report)
        {
            UpdateIndex();
            UtilRuntime.SetPref(PrefSetting.Encryption, PrefUseOption.Build, (int) UtilEditor.Settings.Encryption);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initializes the event subscription needed for this to work in editor.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
        }


        /// <summary>
        /// Runs when the editor has updated.
        /// </summary>
        private static void OnEditorUpdate()
        {
            // If the user is about to enter play-mode, update the index, otherwise leave it be. 
            if (!EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying) return;
            UpdateIndex();
        }


        /// <summary>
        /// Updates the index with all the save manager asset scriptable objects in the project.
        /// </summary>
        [MenuItem("Tools/Carter Games/Save Manager/Update Asset Index", priority = 17)]
        public static void UpdateIndex()
        {
            var foundAssets = new List<SaveManagerAsset>();
            var asset = AssetDatabase.FindAssets(AssetFilter, null);
            
            if (asset == null || asset.Length <= 0) return;

            foreach (var assetInstance in asset)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetInstance);
                var assetObj = (SaveManagerAsset) AssetDatabase.LoadAssetAtPath(assetPath, typeof(SaveManagerAsset));
                
                // Doesn't include editor only or the index itself.
                if (assetObj == null) continue;
                if (assetObj.GetType() == typeof(AssetIndex) || assetObj.GetType() == typeof(SettingsAssetEditor)) continue;
                foundAssets.Add((SaveManagerAsset) AssetDatabase.LoadAssetAtPath(assetPath, typeof(SaveManagerAsset)));
            }
            
            EditorGUI.BeginChangeCheck();
            UtilEditor.AssetIndex.SetLookup(foundAssets);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(UtilEditor.AssetIndex);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}