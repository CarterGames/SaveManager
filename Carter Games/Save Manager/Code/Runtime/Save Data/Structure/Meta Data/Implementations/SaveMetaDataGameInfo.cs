using System;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    public class SaveMetaDataGameInfo : ISaveMetaData
    {
        public string Key => "$game_info";
        
        public bool CanWriteMetaData => SmAssetAccessor.GetAsset<DataAssetSettings>().UseMetaDataGameInfo;
        
        public JObject GetMetaData()
        {
            return new JObject
            {
                ["$version"] = $"{Application.version}",
                ["$save_date"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"),
            };
        }
    }
}