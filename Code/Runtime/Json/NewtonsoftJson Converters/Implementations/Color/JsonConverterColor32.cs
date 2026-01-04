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

using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Converter for <see cref="Color32"/>
    /// </summary>
    [Preserve]
    public sealed class JsonConverterColor32 : SmJsonConverterBase<Color32>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Reads from the json into the value.
        /// </summary>
        /// <param name="value">The value to read json for.</param>
        /// <param name="name">The name of the field to read.</param>
        /// <param name="reader">The reader</param>
        /// <param name="serializer">The serializer</param>
        protected override void ReadFromJson(ref Color32 value, string name, JsonReader reader, JsonSerializer serializer)
        {
            switch (name)
            {
                case nameof(value.r):
                    value.r = ReadAsByte(reader).GetValueOrDefault(0);
                    break;
                case nameof(value.g):
                    value.g = ReadAsByte(reader).GetValueOrDefault(0);
                    break;
                case nameof(value.b):
                    value.b = ReadAsByte(reader).GetValueOrDefault(0);
                    break;
                case nameof(value.a):
                    value.a = ReadAsByte(reader).GetValueOrDefault(0);
                    break;
            }
        }


        /// <summary>
        /// Gets the values to write into json from the value.
        /// </summary>
        /// <param name="value">The value to write from.</param>
        /// <param name="serializer">The serializer</param>
        /// <returns>A collection of values to store in JSON.</returns>
        protected override IEnumerable<KeyValuePair<string, object>> WriteToJson(Color32 value, JsonSerializer serializer)
        {
            return new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("r", value.r),
                new KeyValuePair<string, object>("g", value.g),
                new KeyValuePair<string, object>("b", value.b),
                new KeyValuePair<string, object>("a", value.a),
            };
        }
        
        
        private byte? ReadAsByte(JsonReader reader)
        {
            return checked((byte)(reader.ReadAsInt32() ?? 0));
        }
    }
}