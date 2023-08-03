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
    /// Handles references to scriptable objects in the asset that need generating without user input etc.
    /// </summary>
    public static class ScriptableRefSM
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        // Asset Paths
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        private static readonly string AssetIndexPath = $"{AssetBasePath}/Carter Games/{AssetName}/Resources/Asset Index.asset";
        private static readonly string SettingsAssetPath = $"{AssetBasePath}/Carter Games/{AssetName}/Data/Runtime Settings.asset";
        private static readonly string EditorSettingsAssetPath = $"{AssetBasePath}/Carter Games/{AssetName}/Data/Editor Settings.asset";
        private static readonly string CapturesObjectAssetPath = $"{AssetBasePath}/Carter Games/{AssetName}/Data/Save Profiles Container.asset";
        private static readonly string SaveDataPath = $"{AssetBasePath}/Carter Games/{AssetName}/Data/Save Data.asset";
        private static readonly string EncryptionKeyAssetPath = $"{AssetBasePath}/Carter Games/{AssetName}/Data/Encryption Key.asset";
        

        // Asset Filters
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        private static readonly string RuntimeSettingsFilter = $"t:{typeof(CarterGames.Assets.SaveManager.SettingsAssetRuntime).FullName}";
        private static readonly string EditorSettingsFilter = $"t:{typeof(CarterGames.Assets.SaveManager.Editor.SettingsAssetEditor).FullName}";
        private static readonly string AssetIndexFilter = $"t:{typeof(CarterGames.Assets.SaveManager.AssetIndex).FullName}";
        private static readonly string SaveProfilesStoreFilter = $"t:{typeof(CarterGames.Assets.SaveManager.Editor.SaveProfilesStore).FullName}";
        private static readonly string SaveDataFilter = $"t:{typeof(CarterGames.Assets.SaveManager.SaveData).FullName}";
        private static readonly string SaveDataEncryptionKeyFilter = $"t:{typeof(CarterGames.Assets.SaveManager.EncryptionKeyAsset).FullName}";
        
        
        // Asset Caches
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        private static CarterGames.Assets.SaveManager.SettingsAssetRuntime settingsAssetRuntimeCache;
        private static CarterGames.Assets.SaveManager.Editor.SettingsAssetEditor settingsAssetEditorCache;
        private static CarterGames.Assets.SaveManager.AssetIndex assetIndexCache;
        private static CarterGames.Assets.SaveManager.Editor.SaveProfilesStore saveProfilesStoreCache;
        private static CarterGames.Assets.SaveManager.SaveData saveDataCache;
        private static CarterGames.Assets.SaveManager.EncryptionKeyAsset encryptionKeyAssetCache;
        
        
        // SerializedObject Caches
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        private static SerializedObject settingsAssetRuntimeObjectCache;
        private static SerializedObject settingsAssetEditorObjectCache;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        // Helper Properties
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the path where the asset code is located.
        /// </summary>
        private static string AssetBasePath => FileEditorUtilSM.AssetBasePath;
        
        
        /// <summary>
        /// Gets the asset name stored in the file util editor class.
        /// </summary>
        private static string AssetName => FileEditorUtilSM.AssetName;

        
        // Asset Properties
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The asset index for the asset.
        /// </summary>
        public static CarterGames.Assets.SaveManager.AssetIndex AssetIndex =>
            FileEditorUtilSM.CreateSoGetOrAssignAssetCache(ref assetIndexCache, AssetIndexFilter, AssetIndexPath, AssetName, $"{AssetName}/Resources/Asset Index.asset");
        
        
        /// <summary>
        /// The editor settings for the asset.
        /// </summary>
        public static CarterGames.Assets.SaveManager.Editor.SettingsAssetEditor EditorSettings =>
            FileEditorUtilSM.CreateSoGetOrAssignAssetCache(ref settingsAssetEditorCache, EditorSettingsFilter, EditorSettingsAssetPath, AssetName, $"{AssetName}/Data/Editor Settings.asset");
        
        
        /// <summary>
        /// The runtime settings for the asset.
        /// </summary>
        public static CarterGames.Assets.SaveManager.SettingsAssetRuntime RuntimeSettings =>
            FileEditorUtilSM.CreateSoGetOrAssignAssetCache(ref settingsAssetRuntimeCache, RuntimeSettingsFilter, SettingsAssetPath, AssetName, $"{AssetName}/Data/Runtime Settings.asset");
        
        
        /// <summary>
        /// The save profiles for the asset.
        /// </summary>
        public static CarterGames.Assets.SaveManager.Editor.SaveProfilesStore SaveProfiles =>
            FileEditorUtilSM.CreateSoGetOrAssignAssetCache(ref saveProfilesStoreCache, SaveProfilesStoreFilter, CapturesObjectAssetPath, AssetName, $"{AssetName}/Data/Save Profiles Container.asset");

        
        /// <summary>
        /// The save data for the asset.
        /// </summary>
        public static CarterGames.Assets.SaveManager.SaveData SaveData =>
            FileEditorUtilSM.CreateSoGetOrAssignAssetCache(ref saveDataCache, SaveDataFilter, SaveDataPath, AssetName, $"{AssetName}/Data/Save Data.asset");
        
        
        /// <summary>
        /// The encryption key asset for the asset.
        /// </summary>
        public static CarterGames.Assets.SaveManager.EncryptionKeyAsset EncryptionKey =>
            FileEditorUtilSM.CreateSoGetOrAssignAssetCache(ref encryptionKeyAssetCache, SaveDataEncryptionKeyFilter, EncryptionKeyAssetPath, AssetName, $"{AssetName}/Data/Encryption Key.asset");
        
        // Object Properties
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The editor SerializedObject for the asset.
        /// </summary>
        public static SerializedObject EditorSettingsObject =>
            FileEditorUtilSM.CreateGetOrAssignSerializedObjectCache(ref settingsAssetEditorObjectCache, EditorSettings);
        
        
        /// <summary>
        /// The runtime SerializedObject for the asset.
        /// </summary>
        public static SerializedObject RuntimeSettingsObject =>
            FileEditorUtilSM.CreateGetOrAssignSerializedObjectCache(ref settingsAssetRuntimeObjectCache, RuntimeSettings);
        
        // Assets Initialized Check
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if all the assets needed for the asset to function are in the project at the expected paths.
        /// </summary>
        public static bool HasAllAssets =>
            File.Exists(AssetIndexPath) && File.Exists(SettingsAssetPath) &&
            File.Exists(EditorSettingsAssetPath) && File.Exists(EncryptionKeyAssetPath) &&
            File.Exists(SaveDataPath) && File.Exists(CapturesObjectAssetPath);
    }
}