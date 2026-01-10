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
    /// Converter for <see cref="Vector4"/>
    /// </summary>
    [Preserve]
    public sealed class JsonConverterVector4 : SmJsonConverterBase<Vector4>
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
        protected override void ReadFromJson(ref Vector4 value, string name, JsonReader reader, JsonSerializer serializer)
        {
            switch (name)
            {
                case nameof(value.x):
                    value.x = (float)reader.ReadAsDouble().GetValueOrDefault(0d);
                    break;
                case nameof(value.y):
                    value.y = (float)reader.ReadAsDouble().GetValueOrDefault(0d);
                    break;
                case nameof(value.z):
                    value.z = (float)reader.ReadAsDouble().GetValueOrDefault(0d);
                    break;
                case nameof(value.w):
                    value.w = (float)reader.ReadAsDouble().GetValueOrDefault(0d);
                    break;
            }
        }

        
        /// <summary>
        /// Gets the values to write into json from the value.
        /// </summary>
        /// <param name="value">The value to write from.</param>
        /// <param name="serializer">The serializer</param>
        /// <returns>A collection of values to store in JSON.</returns>
        protected override IEnumerable<KeyValuePair<string, object>> WriteToJson(Vector4 value, JsonSerializer serializer)
        {
            return new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("x", value.x),
                new KeyValuePair<string, object>("y", value.y),
                new KeyValuePair<string, object>("z", value.z),
                new KeyValuePair<string, object>("w", value.w),
            };
        }
    }
}