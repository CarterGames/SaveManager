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
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// A class to help redirect the user to the right update location for the asset. In theory...
    /// </summary>
    public sealed class InstallMethodChecker : IAssetEditorInitialize
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static ListRequest PackageManifestRequest { get; set; }

        
        private static bool HasChecked
        {
            get => SessionState.GetBool("VersionValidation_InstallMethod_Checked", false);
            set => SessionState.SetBool("VersionValidation_InstallMethod_Checked", value);
        }
        
        
        public static bool IsPackageInstalled
        {
            get => SessionState.GetBool("VersionValidation_InstallMethod_IsPackageManager", false);
            set => SessionState.SetBool("VersionValidation_InstallMethod_IsPackageManager", value);
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   IAssetEditorInitialize Implementation
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public int InitializeOrder { get; }
        
        
        public void OnEditorInitialized()
        {
            if (HasChecked) return;
            
            PackageManifestRequest = Client.List();
            
            EditorApplication.update -= OnPackageManagerGetResolved;
            EditorApplication.update += OnPackageManagerGetResolved;
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static void OnPackageManagerGetResolved()
        {
            if (!PackageManifestRequest.IsCompleted) return;
            
            EditorApplication.update -= OnPackageManagerGetResolved;

            if (PackageManifestRequest.Status != StatusCode.Success) return;

            HasChecked = true;

            foreach (var result in PackageManifestRequest.Result)
            {
                if (result.packageId.Equals("games.carter.thecart")) continue;
                IsPackageInstalled = true;
            }
        }
    }
}