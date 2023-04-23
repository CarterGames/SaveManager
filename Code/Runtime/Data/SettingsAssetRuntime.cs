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

using CarterGames.Assets.SaveManager.Encryption;
using CarterGames.Assets.SaveManager.Extensions;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles the runtime settings for the save manager asset.
    /// </summary>
    [CreateAssetMenu(fileName = "Runtime Settings", menuName = "Carter Games/Save Manager/Runtime Settings Asset", order = 2)]
    public sealed class SettingsAssetRuntime : SaveManagerAsset
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        [SerializeField] private string defaultSavePath = "%Application.persistentDataPath%/save.sf";
        [SerializeField] private string defaultSavePathWeb = "/idbfs/%productName%-%companyName%/save.sf";

        [SerializeField] private SaveData saveDataAsset;

        [SerializeField] private EncryptionOption encryptionOption;
        [SerializeField] private bool prettify;
        [SerializeField] private bool autoLoadOnEntry = true;
        [SerializeField] private bool autoSaveOnExit = true;
        [SerializeField] private bool showLogs;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets the default save path per platform, you cannot edit this.
        /// </summary>
        public string DefaultSavePath
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return defaultSavePathWeb.ParseSavePath();
#else
                return defaultSavePath.ParseSavePath();
#endif
            }
        }
        

        /// <summary>
        /// Gets the save path for the game save data.
        /// </summary>
        public string SavePath
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return defaultSavePathWeb.ParseSavePath();
#else
                return defaultSavePath.ParseSavePath();
#endif
            }
        }


        /// <summary>
        /// The save data asset for the game save.
        /// </summary>
        public SaveData SaveData
        {
            get
            {
                if (saveDataAsset != null) return saveDataAsset;
                saveDataAsset = AssetAccessor.GetAsset<SaveData>();
                return saveDataAsset;
            }
        }
        
        
        /// <summary>
        /// The current encryption option selected by the user.
        /// </summary>
        public EncryptionOption Encryption => encryptionOption;
        
        
        /// <summary>
        /// Defines if the asset will throw log messages at you or not. 
        /// </summary>
        public bool ShowLogs => showLogs;
        
        
        /// <summary>
        /// Defines if the json formatting is laid out nicely or not.
        /// </summary>
        public bool Prettify => prettify;

        public bool AutoLoad => autoLoadOnEntry;
        public bool AutoSave => autoSaveOnExit;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnValidate()
        {
            if (defaultSavePath.Length <= 0)
            {
                defaultSavePath = "%Application.persistentDataPath%/save.sf";
                defaultSavePathWeb = "/idbfs/%productName%-%companyName%/save.sf";
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initializes the asset when called.
        /// </summary>
        public void Initialize()
        {
            OnValidate();
        }
    }
}