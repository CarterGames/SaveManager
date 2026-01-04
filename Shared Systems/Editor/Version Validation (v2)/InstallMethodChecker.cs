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

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace CarterGames.Shared.SaveManager.Editor
{
    /// <summary>
    /// Tries to check for the install method of the asset.
    /// </summary>
    public class InstallMethodChecker : IAssetEditorInitialize
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The result of getting the packages in the project.
        /// </summary>
        private static ListRequest PackageManifestRequest { get; set; }

        
        /// <summary>
        /// Gets if the system has checked this session.
        /// </summary>
        private static bool HasChecked
        {
            get => SessionState.GetBool("VersionValidation_InstallMethod_Checked", false);
            set => SessionState.SetBool("VersionValidation_InstallMethod_Checked", value);
        }
        
        
        /// <summary>
        /// Gets if the package was detected installed via the package manager this session.
        /// </summary>
        public static bool IsPackageInstalled
        {
            get => SessionState.GetBool("VersionValidation_InstallMethod_IsPackageManager", false);
            private set => SessionState.SetBool("VersionValidation_InstallMethod_IsPackageManager", value);
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   IAssetEditorInitialize Implementation
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public int InitializeOrder { get; } = 0;
        
        
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