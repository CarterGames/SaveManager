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

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// The info used in the version validation system.
    /// </summary>
    public static class VersionInfo
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The Url to request the versions from.
        /// </summary>
        public const string ValidationUrl = "https://carter.games/validation/versions.json";
        
        
        /// <summary>
        /// The download Url for the latest version of this package.
        /// </summary>
        public const string DownloadBaseUrl = "https://github.com/CarterGames/SaveManager/releases/tag/";
        
        
        /// <summary>
        /// The key of the package to get from the JSON blob.
        /// </summary>
        public const string Key = "Save Manager";


        /// <summary>
        /// The version string for the package.
        /// </summary>
        public static string ProjectVersionNumber => AssetVersionData.VersionNumber;
    }
}