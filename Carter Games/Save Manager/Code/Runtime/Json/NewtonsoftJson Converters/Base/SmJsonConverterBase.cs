using System;
using System.Collections.Generic;
using CarterGames.Assets.SaveManager.NewtonsoftJson_Converters;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace CarterGames.Assets.SaveManager
{
    [Preserve]
    public abstract class SmJsonConverterBase<T> : JsonConverter, IAssetJsonConverter where T : new()
    {
        public Type TargetType => typeof(T);
        public JsonConverter Converter => this;
        
        protected abstract void ReadFromJson(ref T value, string name, JsonReader reader, JsonSerializer serializer);
        protected abstract IEnumerable<KeyValuePair<string, object>> WriteToJson(T value, JsonSerializer serializer);
        
        
        public override bool CanConvert(Type objectType)
        {
            // Disable if the setting to use the provided converters is false.
            if (!SmAssetAccessor.GetAsset<DataAssetSettings>().UseJsonConverters) return false;
            
            return objectType == typeof(T)
                   || (objectType.IsGenericType
                       && objectType.GetGenericTypeDefinition() == typeof(Nullable<>)
                       && objectType.GenericTypeArguments[0] == typeof(T));
        }

        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? null
                    : (object)default(T);
            }

            var result = existingValue is T value ? new T() : default(T);
            
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject) break;
                
                if (reader.TokenType != JsonToken.PropertyName)
                {
                    reader.Skip();
                    continue;
                }
                
                if (reader.Value == null)
                {
                    reader.Skip();
                    continue;
                }

                ReadFromJson(ref result, reader.Value.ToString(), reader, serializer);
            }

            return result;
        }

        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            writer.WriteStartObject();

            foreach (var entry in WriteToJson((T) value, serializer))
            {
                writer.WritePropertyName(entry.Key);
                writer.WriteValue(entry.Value);
            }
            
            writer.WriteEndObject();
        }
    }
}