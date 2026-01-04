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

using System;

namespace CarterGames.Shared.SaveManager.Editor
{
    /// <summary>
    /// Handles checking for the latest version.
    /// </summary>
    public static class VersionChecker
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The download URL for the latest version.
        /// </summary>
        public static string DownloadURL => VersionInfo.DownloadBaseUrl + VersionsPacket.Version;
        

        /// <summary>
        /// Gets if the latest version is this version.
        /// </summary>
        public static bool IsLatestVersion => VersionsPacket.VersionNumber.Equals(new Version(VersionInfo.ProjectVersionNumber));
        
        
        /// <summary>
        /// Gets if the version here is higher that the latest version.
        /// </summary>
        public static bool IsNewerVersion => new Version(VersionInfo.ProjectVersionNumber).CompareTo(VersionsPacket.VersionNumber) > 0;
        
        
        /// <summary>
        /// Gets the version data downloaded.
        /// </summary>
        public static VersionPacketSuccess VersionsPacket { get; private set; }

        
        /// <summary>
        /// The latest version string.
        /// </summary>
        public static string LatestVersionNumberString => VersionsPacket.Version;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises when the data has been downloaded.
        /// </summary>
        public static Evt ResponseReceived { get; private set; } = new Evt();
        
        
        /// <summary>
        /// Raises when the request failed for some reason.
        /// </summary>
        public static Evt<VersionPacketError> ErrorReceived { get; private set; } = new Evt<VersionPacketError>();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the latest version data when called.
        /// </summary>
        public static void GetLatestVersions()
        {
            RequestLatestVersionData();
        }


        /// <summary>
        /// Makes the web request & handles the response.
        /// </summary>
        private static void RequestLatestVersionData()
        {
            VersioningAPI.Query(OnSuccess, OnFailed);
            return;
            
            void OnSuccess(VersionPacketSuccess data)
            {
                VersionsPacket = data;
                ResponseReceived.Raise();
            }

            void OnFailed(VersionPacketError errorPacket)
            {
                ErrorReceived.Raise(errorPacket);
            }
        }
    }
}