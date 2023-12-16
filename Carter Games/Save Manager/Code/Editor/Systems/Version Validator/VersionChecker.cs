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

using CarterGames.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace CarterGames.Assets.SaveManager.Editor
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
        public static string DownloadURL => VersionInfo.DownloadBaseUrl + Versions.Data.Version;
        

        /// <summary>
        /// Gets if the latest version is this version.
        /// </summary>
        public static bool IsLatestVersion => Versions.Data.Match(VersionInfo.ProjectVersionNumber);

        
        /// <summary>
        /// Gets if the version here is higher that the latest version.
        /// </summary>
        public static bool IsNewerVersion => Versions.Data.IsHigherVersion(VersionInfo.ProjectVersionNumber);
        
        
        /// <summary>
        /// Gets the version data downloaded.
        /// </summary>
        public static VersionPacket Versions { get; private set; }

        
        /// <summary>
        /// The latest version string.
        /// </summary>
        public static string LatestVersionNumberString => Versions.Data.Version;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises when the data has been downloaded.
        /// </summary>
        public static Evt ResponseReceived { get; private set; } = new Evt();

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
            var request = UnityWebRequest.Get(VersionInfo.ValidationUrl);
            var async = request.SendWebRequest();

            async.completed += (a) =>
            {
                if (request.result != UnityWebRequest.Result.Success) return;

                Versions = JsonUtility.FromJson<VersionPacket>(request.downloadHandler.text);
                ResponseReceived.Raise();
            };
        }
    }
}