using Newtonsoft.Json.Linq;

namespace CarterGames.Assets.SaveManager
{
    public interface ISaveMetaData
    {
        string Key { get; }
        bool CanWriteMetaData { get; }
        JObject GetMetaData();
    }
}