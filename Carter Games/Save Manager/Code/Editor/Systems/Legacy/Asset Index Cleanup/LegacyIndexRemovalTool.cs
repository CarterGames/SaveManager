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
using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// A helper class to remove the old asset index if it exists in the project still.
    /// As of (2.0.9) it was moved into the asset folder structure, so the external one is not used anymore.
    /// </summary>
    public static class LegacyIndexRemovalTool
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The path of the old index to clear.
        /// </summary>
        private const string LegacyIndexPath = "Assets/Resources/Carter Games/Save Manager/Asset Index.asset";

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Returns if the old index is still in place.
        /// </summary>
        /// <returns>Bool</returns>
        private static bool HasLegacyIndex()
        {
            return File.Exists(LegacyIndexPath);
        }


        /// <summary>
        /// Tries to remove the old index and pathing where possible
        /// </summary>
        /// <remarks>Doesn't delete Carter Games folder in-case other assets still use it.</remarks>
        public static void TryRemoveOldIndex()
        {
            if (!HasLegacyIndex()) return;
            
            Directory.Delete("Assets/Resources/Carter Games/Save Manager/", true);
            AssetDatabase.Refresh();

            if (Directory.GetFiles("Assets/Resources/Carter Games/").Length <= 1)
            {
                FileUtil.DeleteFileOrDirectory("Assets/Resources/Carter Games");
            }

            AssetDatabase.Refresh();
        }
    }
}