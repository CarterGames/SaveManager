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

using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles the auto saving of the game if the setting is enabled.
    /// </summary>
    public class AutoSave : MonoBehaviour
    {
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Fields
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        private static AutoSave instance;

        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Methods
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// Initializes the auto saving setup for use if the setting is enabled.
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void SetupInstance()
        {
            if (!AssetAccessor.GetAsset<SettingsAssetRuntime>().AutoSave) return;
            if (instance != null) return;

            var obj = new GameObject("Auto Save (Save Manager)");
            obj.AddComponent<AutoSave>();
            instance = obj.GetComponent<AutoSave>();
            DontDestroyOnLoad(obj);
        }

        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Unity Methods
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// Runs when the application quits
        /// </summary>
        private void OnApplicationQuit()
        {
            if (!AssetAccessor.GetAsset<SettingsAssetRuntime>().AutoSave) return;
            SaveManager.Save(false);
        }


        /// <summary>
        /// Runs when the application loses focus, like clicking off the game application when its running. 
        /// </summary>
        /// <param name="hasFocus">If the user is focused on the application.</param>
        private void OnApplicationFocus(bool hasFocus)
        {
#if !UNITY_EDITOR
            if (!AssetAccessor.GetAsset<SettingsAssetRuntime>().AutoSave) return;
            if (hasFocus) return;
            SaveManager.Save(false);
#endif
        }
    }
}