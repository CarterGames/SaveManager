using System;
using System.Collections.Generic;
using CarterGames.Assets.SaveManager.Helpers;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Windows;

namespace CarterGames.Assets.SaveManager.Legacy
{
    /// <summary>
    /// Attempts to convert a 2.x save file to a 3.x format.
    /// </summary>
    public static class LegacySaveManager
    {
        private const string EditorSavePath = "%Application.persistentDataPath%/EditorSave/save.sf";
        private const string SavePath = "%Application.persistentDataPath%/save.sf";
        private const string SavePathWeb = "/idbfs/%productName%-%companyName%/save.sf";

        private const string ConvertedEditorSavePath = "%Application.persistentDataPath%/EditorSave/Legacy/save.sf";
        private const string ConvertedRuntimeSavePath = "%Application.persistentDataPath%/Legacy/save.sf";
        private const string ConvertedSavePathWeb = "/idbfs/%productName%-%companyName%/Legacy/save.sf";
        

        private static readonly DataLocationLocalFile LocalFileHandler = new DataLocationLocalFile();
        private const string DictionaryIdentifier = "{\n          \"list\": [\n            {";
        
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



        private static bool HasLegacyFileToLoad()
        {
            return File.Exists(ActiveSavePath) && !File.Exists(ConvertedSavePath);
        }
        

        private static string LoadLegacyFile()
        {
            return LocalFileHandler.LoadFromLocation(ActiveSavePath);
        }


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
                Debug.Log(updatedData);

                updatedData = LegacyDictionaryHelper.ConvertAnyLegacyDictionaries(updatedData);
                Debug.Log(updatedData);
                
                CreateConvertedFile(legacyFileData);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }


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


        private static void CreateConvertedFile(string loadedData)
        {
            LocalFileHandler.SaveToLocation(ConvertedSavePath, loadedData);
        }

        
        // public static void ConvertLegacySD(JToken token)
        // {
        //     var jo = token;
        //     
        //     // Call the method to get all values
        //     GetAllValues(token);
        //
        //     Debug.LogError("--- FINAL");
        //     Debug.LogError(jo.ToString());
        //
        //     return;
        //     void GetAllValues(JToken tokenValue)
        //     {
        //         if (tokenValue.Type == JTokenType.Object || tokenValue.Type == JTokenType.Array)
        //         {
        //             foreach (JToken child in tokenValue.Children())
        //             {
        //                 GetAllValues(child);
        //             }
        //         }
        //         else
        //         {
        //             foreach (var entry in tokenValue.SelectTokens("$..list"))
        //             {
        //                 var key = entry.Path.Replace(".list", string.Empty).Split('.').Last();
        //                 Debug.LogError(key);
        //                 var result = ConvertLegacyDictionaries(JArray.FromObject(entry));
        //                 // entry.obj.Remove();
        //                 jo[key] = result;
        //             }
        //         }
        //     }
        // }
        //
        //
        // public static void ParseLegacySD(Type targetType, JToken json)
        // {
        //     var hasDic = json.ToString().Contains("\"list\": [");
        //     Debug.LogError(hasDic);
        //
        //     if (!hasDic) return;//
        //
        //     ConvertLegacySD(json);
        // }
        //
        //
        // public static JObject ConvertLegacyDictionaries(JArray data)
        // {
        //     var toRead = data;
        //     var entryObj = new JObject();
        //     
        //     foreach (var entry in toRead)
        //     {
        //         entryObj[entry["key"].ToString()] = entry["value"];
        //     }
        //     
        //     Debug.LogError("-- CONVERTED");
        //     Debug.LogError(entryObj.ToString());
        //
        //     return entryObj;
        // }
    }
}