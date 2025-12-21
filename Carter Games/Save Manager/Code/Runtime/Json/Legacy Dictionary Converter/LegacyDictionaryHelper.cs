using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A helper class to handle converting older SM2.x saves where the Serializable Dictionary type was used to port over correctly to the 3.X format.
    /// </summary>
    public static class LegacyDictionaryHelper 
    {
        public static JToken ConvertAnyLegacyDictionaries(JToken tokenValue)
        {
            if (!TryGetAllToConvert(tokenValue, out var entries)) return tokenValue;
            return entries;
        }


        private static bool TryGetAllToConvert(JToken token, out JObject updated)
        {
            var jo = token.DeepClone().Value<JObject>();
            
            // Call the method to get all values
            GetAllValues(token);

            updated = jo;
            return true;
            
            void GetAllValues(JToken tokenValue)
            {
                if (tokenValue.Type == JTokenType.Object || tokenValue.Type == JTokenType.Array)
                {
                    foreach (JToken child in tokenValue.Children())
                    {
                        GetAllValues(child);
                    }
                }
                else
                {
                    foreach (var entry in tokenValue.SelectTokens("$..list"))
                    {
                        jo.ReplacePath(entry.Parent.Parent.Path, ConvertLegacyDictionaries(entry.Value<JArray>()));
                    }
                }
            }
        }


        private static JObject ConvertLegacyDictionaries(JArray data)
        {
            var toRead = data;
            var entryObj = new JObject();
            
            foreach (var entry in toRead)
            {
                entryObj[entry["key"].ToString()] = entry["value"];
            }
            
            return entryObj;
        }


        private static JObject ReplacePath<T>(this JToken root, string path, T newValue)
        {
            if (root == null || path == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var value in root.SelectTokens(path).ToList())
            {
                if (value == root)
                {
                    root = JToken.FromObject(newValue);
                }
                else
                {
                    value.Replace(JToken.FromObject(newValue));
                }
            }

            return (JObject)root;
        }
    }
}