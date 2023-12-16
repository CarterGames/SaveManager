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
    /// A data class to hold a x.x.x version number for comparisons.
    /// </summary>
    [Serializable]
    public class VersionNumber
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private int major;
        [SerializeField] private int minor;
        [SerializeField] private int patch;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The major version number.
        /// </summary>
        public int Major => major;
        
        
        /// <summary>
        /// The minor version number.
        /// </summary>
        public int Minor => minor;
        
        
        /// <summary>
        /// The patch version number.
        /// </summary>
        public int Patch => patch;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The blank constructor.
        /// </summary>
        public VersionNumber() { }

        
        /// <summary>
        /// Makes a new version number with the string entered.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public VersionNumber(string value)
        {
            var split = value.Split('.');

            if (split.Length != 3) return;

            major = int.Parse(split[0]);
            minor = int.Parse(split[1]);
            patch = int.Parse(split[2]);
        }
    }
}