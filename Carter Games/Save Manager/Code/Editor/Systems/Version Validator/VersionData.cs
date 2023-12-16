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
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// A copy of the Json data for each entry stored on the server.
    /// </summary>
    [Serializable]
    public class VersionData
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private string key;
        [SerializeField] private string version;
        [SerializeField] private string releaseDate;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The key for the entry.
        /// </summary>
        public string Key
        {
            get => key;
            set => key = value;
        }
        
        
        /// <summary>
        /// The version for the entry.
        /// </summary>
        public string Version
        {
            get => version;
            set => version = value;
        }        
        
        
        /// <summary>
        /// The release date for the entry.
        /// </summary>
        public string ReleaseDate
        {
            get => releaseDate;
            set => releaseDate = value;
        }


        /// <summary>
        /// The version number for the entry.
        /// </summary>
        public VersionNumber VersionNumber => new VersionNumber(Version);
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets if the entry version number matches the entered string.
        /// </summary>
        /// <param name="toCompare">The version string to compare.</param>
        /// <returns>If the entry is a match or not on all values (major/minor/patch).</returns>
        public bool Match(string toCompare)
        {
            var aVN = VersionNumber;
            var bVN = new VersionNumber(toCompare);

            return aVN.Major.Equals(bVN.Major) && aVN.Minor.Equals(bVN.Minor) && aVN.Patch.Equals(bVN.Patch);
        }
                
        
        /// <summary>
        /// Gets if the entry is a higher version than the converted version.
        /// </summary>
        /// <param name="toCompare">The version string to compare.</param>
        /// <returns>If the entry is greater on any (major/minor/patch) value.</returns>
        public bool IsHigherVersion(string toCompare)
        {
            var aVN = VersionNumber;
            var bVN = new VersionNumber(toCompare);

            if (Match(toCompare))
            {
                return false;
            }
            
            return (aVN.Major < bVN.Major) || (aVN.Minor < bVN.Minor) || (aVN.Patch < bVN.Patch);
        }
    }
}