using CarterGames.Shared.SaveManager;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    public class SaveMetaDataSystemInfo : ISaveMetaData
    {
        public string Key => "$system_info";

        public bool CanWriteMetaData => SmAssetAccessor.GetAsset<DataAssetSettings>().UseMetaDataSystemInfo;

        public JObject GetMetaData()
        {
            return new JObject
            {
                ["$os"] = $"{SystemInfo.operatingSystem}",
                ["$cpu"] = $"{SystemInfo.processorType}",
                ["$ram"] = $"{SystemInfo.systemMemorySize}MB",
                ["$gpu"] = $"{SystemInfo.graphicsDeviceName} ({SystemInfo.graphicsMemorySize}MB)",
            };
        }
    }
}