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

using System.Linq;
using CarterGames.Assets.SaveManager.Slots;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Generates the complete save data to write to the save.
    /// </summary>
    public static class SaveStructure
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets all the global save data to write.
        /// </summary>
        /// <returns>The global data to write.</returns>
        private static JArray GenerateGlobalSaveData()
        {
            var valuesJson = new JArray();

            foreach (var entry in SaveObjectController.GetGlobalSaveFields().ToArray())
            {
                if (string.IsNullOrEmpty(entry.key))
                {
                    // Error, no save key defined for save value.
                    SmDebugLogger.LogWarning(SaveManagerErrorCode.NoSaveKeyAssigned.GetErrorMessageFormat(entry));
                    continue;
                }

                var defObj = JObject.FromObject(entry, JsonHelper.SaveManagerSerializer);
                
                var obj = new JObject
                {
                    ["$key"] = entry.key,
                    ["$value"] = defObj["value"],
                    ["$type"] = entry.ValueType.ToString()
                };

                if (defObj.SelectToken("hasDefaultValue") != null)
                {
                    if (defObj.SelectToken("hasDefaultValue").Value<bool>())
                    {
                        obj["$default"] = defObj["defaultValue"];
                    }
                }

                valuesJson.Add(obj);
            }

            return valuesJson;
        }


        /// <summary>
        /// Gets all the metadata to write.
        /// </summary>
        /// <returns>A JObject with the data.</returns>
        private static JObject GenerateMetaSaveData()
        {
            var metaDataClasses = AssemblyHelper.GetClassesOfType<ISaveMetaData>(false).Where(t => t.CanWriteMetaData);

            // Skip if no meta data is enabled.
            if (!metaDataClasses.Any()) return null;
            
            var obj = new JObject();
            
            foreach (var entry in metaDataClasses)
            {
                var data = entry.GetMetaData();
                if (data == null) continue;
                obj.Add(entry.Key, data);
            }

            return obj;
        }
        
        
        /// <summary>
        /// Generates the complete save data to save.
        /// </summary>
        /// <returns>A JObject with all the data to write.</returns>
        public static JObject GenerateSaveData()
        {
            var structure = new JObject();

            var contentObj = new JObject()
            {
                ["$global"] = GenerateGlobalSaveData()
            };

            if (SmAssetAccessor.GetAsset<DataAssetSettings>().UseSaveSlots)
            {
                contentObj.Add("$slots", SaveSlotManager.INTERNAL_GetSlotDataToSave());
            }
           
            structure.Add("$content", contentObj);

            var metaData = GenerateMetaSaveData();

            if (metaData != null)
            {
                structure.Add("$meta_data", metaData);
            }
            
            return structure;
        }
    }
}