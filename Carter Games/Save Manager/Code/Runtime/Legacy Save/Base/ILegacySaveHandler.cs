using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CarterGames.Assets.SaveManager.Legacy
{
    public interface ILegacySaveHandler
    {
        JToken ProcessLegacySaveData(JToken loadedJson, IReadOnlyDictionary<string, JArray> legacyData);
    }
}