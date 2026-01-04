/*
 * Save Manager (3.x)
 * Copyright (c) 2025-2026 Carter Games
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version. 
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. 
 *
 * You should have received a copy of the GNU General Public License along with this program.
 * If not, see <https://www.gnu.org/licenses/>. 
 */

using System;
using System.Collections.Generic;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A base class to handle converting types to and from JSON in custom structures.
    /// Implement from this class if you want to add your own converters for your class types if they do not parse correctly.
    /// </summary>
    /// <typeparam name="T">The type to convert.</typeparam>
    [Preserve]
    public abstract class SmJsonConverterBase<T> : JsonConverter, IAssetJsonConverter where T : new()
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The target type to convert.
        /// </summary>
        public Type TargetType => typeof(T);
        
        
        /// <summary>
        /// The converter reference (for IAssetJsonConverter).
        /// </summary>
        public JsonConverter Converter => this;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Implement to read the value into JSON.
        /// </summary>
        /// <param name="value">The value to read json for.</param>
        /// <param name="name">The name of the field to read.</param>
        /// <param name="reader">The reader</param>
        /// <param name="serializer">The serializer</param>
        protected abstract void ReadFromJson(ref T value, string name, JsonReader reader, JsonSerializer serializer);
        
        
        /// <summary>
        /// Implement to write the JSON for the value.
        /// </summary>
        /// <param name="value">The value to write from.</param>
        /// <param name="serializer">The serializer</param>
        /// <returns>A collection of values to store in JSON.</returns>
        protected abstract IEnumerable<KeyValuePair<string, object>> WriteToJson(T value, JsonSerializer serializer);
        
        
        /// <summary>
        /// Gets if the type can be converted.
        /// </summary>
        /// <param name="objectType">The type to check for.</param>
        /// <returns>Bool</returns>
        public override bool CanConvert(Type objectType)
        {
            // Disable if the setting to use the provided converters is false.
            if (!SmAssetAccessor.GetAsset<DataAssetSettings>().UseJsonConverters) return false;
            
            return objectType == typeof(T)
                   || (objectType.IsGenericType
                       && objectType.GetGenericTypeDefinition() == typeof(Nullable<>)
                       && objectType.GenericTypeArguments[0] == typeof(T));
        }

        
        /// <summary>
        /// Reads the json from the 
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <param name="objectType">The object type</param>
        /// <param name="existingValue">The current value object</param>
        /// <param name="serializer">The serializer</param>
        /// <returns>The object read from the json.</returns>
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

        
        /// <summary>
        /// Writes to JSON the value entered.
        /// </summary>
        /// <param name="writer">The writer</param>
        /// <param name="value">The value object to write</param>
        /// <param name="serializer">The serializer</param>
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