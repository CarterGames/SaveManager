/*
 * Copyright (c) 2025 Carter Games
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

using System.Linq;
using CarterGames.Assets.SaveManager.Slots;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    public static class SaveStructure
    {
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