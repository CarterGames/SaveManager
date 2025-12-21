using System;
using Newtonsoft.Json;

namespace CarterGames.Assets.SaveManager.NewtonsoftJson_Converters
{
    public interface IAssetJsonConverter
    {
        Type TargetType { get; }
        JsonConverter Converter { get; }
    }
}