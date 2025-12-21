using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Legacy
{
    public class LegacySaveHandlerGlobalOnly : ILegacySaveHandler
    {
        public JToken ProcessLegacySaveData(JToken loadedJson, IReadOnlyDictionary<string, JArray> legacyData)
        {
            // Processes each save value based on if its key exists in the global save in 3.x
            // If its now slot data it'll not be transferred in this setup.
            // You'll have to your own handler if you want to load legacy to slots.
            JToken updated = loadedJson;

            var globalDataArray = loadedJson["$content"]["$global"].Value<JArray>();
            
            foreach (var entry in legacyData)
            {
                foreach (var legacySaveValue in entry.Value)
                {
                    var legacySaveValueKey = legacySaveValue["$key"].Value<string>();

                    for (var i = 0; i < globalDataArray.Count; i++)
                    {
                        var currentSaveValue = globalDataArray[i];

                        if (currentSaveValue["$key"].Value<string>() != legacySaveValueKey) continue;

                        var adjusted = currentSaveValue;
                        adjusted["$value"] = legacySaveValue["$value"];

                        if (legacySaveValue["$default"] != null)
                        {
                            adjusted["$default"] = legacySaveValue["$default"];
                        }
                        
                        updated["$content"]["$global"][i] = adjusted;

                        Debug.LogError(updated["$content"]["$global"][i]);
                    }
                }
            }

            return updated;
        }
    }
}