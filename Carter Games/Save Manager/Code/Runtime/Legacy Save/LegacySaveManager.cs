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
using System.Collections.Generic;
using CarterGames.Assets.SaveManager.Helpers;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json.Linq;
using UnityEngine.Windows;

namespace CarterGames.Assets.SaveManager.Legacy
{
    /// <summary>
    /// Attempts to convert a 2.x save file to a 3.x format.
    /// </summary>
    public static class LegacySaveManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string EditorSavePath = "%Application.persistentDataPath%/EditorSave/save.sf";
        private const string SavePath = "%Application.persistentDataPath%/save.sf";
        private const string SavePathWeb = "/idbfs/%productName%-%companyName%/save.sf";

        private const string ConvertedEditorSavePath = "%Application.persistentDataPath%/EditorSave/Legacy/save.sf";
        private const string ConvertedRuntimeSavePath = "%Application.persistentDataPath%/Legacy/save.sf";
        private const string ConvertedSavePathWeb = "/idbfs/%productName%-%companyName%/Legacy/save.sf";
        

        private static readonly DataLocationLocalFile LocalFileHandler = new DataLocationLocalFile();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the active path for the 2.x save.
        /// </summary>
        private static string ActiveSavePath
        {
            get
            {
#if UNITY_EDITOR
                return LocalFileHelper.ParseSavePath(EditorSavePath);
#elif UNITY_WEBGL
                return LocalFileHelper.ParseSavePath(SavePathWeb);
#else
                return LocalFileHelper.ParseSavePath(SavePath);
#endif
            }
        }
        
        
        /// <summary>
        /// Gets the path to save converted 2.x files to.
        /// </summary>
        private static string ConvertedSavePath
        {
            get
            {
#if UNITY_EDITOR
                return LocalFileHelper.ParseSavePath(ConvertedEditorSavePath);
#elif UNITY_WEBGL
                return LocalFileHelper.ParseSavePath(ConvertedSavePathWeb);
#else
                return LocalFileHelper.ParseSavePath(ConvertedRuntimeSavePath);
#endif
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if there is a legacy save (2.x) file to convert.
        /// </summary>
        /// <returns></returns>
        private static bool HasLegacyFileToLoad()
        {
            return File.Exists(ActiveSavePath) && !File.Exists(ConvertedSavePath);
        }
        

        /// <summary>
        /// Loads the legacy save file.
        /// </summary>
        /// <returns>The json stored in the 2.x file.</returns>
        private static string LoadLegacyFile()
        {
            return LocalFileHandler.LoadFromLocation(ActiveSavePath);
        }


        /// <summary>
        /// Tries to load the legacy data.
        /// </summary>
        /// <param name="loadedJson">The loaded JSON to add to (3.x)</param>
        /// <param name="updatedData">The updated data with any legacy save changes.</param>
        /// <returns>Bool</returns>
        public static bool TryLoadLegacySaveData(JToken loadedJson, out JToken updatedData)
        {
            updatedData = loadedJson;
            
            try
            {
                if (!HasLegacyFileToLoad()) return false;
                if (!SmAssetAccessor.GetAsset<DataAssetSettings>().TryPortLegacySave) return false;
                if (!SmAssetAccessor.GetAsset<DataAssetSettings>().HasLegacySaveHandler) return false;

                var legacyFileData = LoadLegacyFile();
                updatedData = SmAssetAccessor.GetAsset<DataAssetSettings>().LegacySaveHandler.ProcessLegacySaveData(loadedJson, GetSaveDataOnly(legacyFileData));
                SmDebugLogger.LogDev($"2.x data load [Step 1]:\n{updatedData}");

                updatedData = LegacyDictionaryHelper.ConvertAnyLegacyDictionaries(updatedData);
                SmDebugLogger.LogDev($"2.x data load [Step 2]:\n{updatedData}");
                
                CreateConvertedFile(legacyFileData);
                return true;
            }
            catch (Exception e)
            {
                SmDebugLogger.LogError(SaveManagerErrorCode.LegacyLoadFailed.GetErrorMessageFormat(e.Message, e.StackTrace));
                return false;
            }
        }


        /// <summary>
        /// Gets the save data from the 2.x save into a 3.x structure.
        /// </summary>
        /// <param name="data">The data to read.</param>
        /// <returns>A collection to use in the 3.x save.</returns>
        private static IReadOnlyDictionary<string, JArray> GetSaveDataOnly(string data)
        {
            var json = JToken.Parse(data);
            var oldSaveLookup = new Dictionary<string, JArray>();

            foreach (var entry in json["list"])
            {
                var saveObjectKey = entry["key"].Value<string>();
                var array = new JArray();
                
                foreach (var objEntry in entry["value"]["list"])
                {
                        var readableValue = JToken.Parse(objEntry["value"].Value<string>());

                        JObject convertedObj;
                        
                        if (readableValue["defaultValue"] != null)
                        {
                            convertedObj = new JObject()
                            {
                                ["$key"] = readableValue["key"],
                                ["$value"] = readableValue["value"],
                                ["$default"] = readableValue["defaultValue"],
                            };
                        }
                        else
                        {
                            convertedObj = new JObject()
                            {
                                ["$key"] = readableValue["key"],
                                ["$value"] = readableValue["value"],
                            };
                        }
                        
                        array.Add(convertedObj);
                }
                
                oldSaveLookup.Add(saveObjectKey, array);
            }

            return oldSaveLookup;
        }


        /// <summary>
        /// Creates a converted save file from the old legacy file.
        /// </summary>
        /// <param name="loadedData">The data to write to the converted file.</param>
        private static void CreateConvertedFile(string loadedData)
        {
            LocalFileHandler.SaveToLocation(ConvertedSavePath, loadedData);
        }
    }
}