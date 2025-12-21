/*
 * Copyright (c) 2025 Carter Games
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Converter for <see cref="Vector3"/>
    /// </summary>
    [Preserve]
    public sealed class JsonConverterVector3 : SmJsonConverterBase<Vector3>
    {
        protected override void ReadFromJson(ref Vector3 value, string name, JsonReader reader, JsonSerializer serializer)
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
            }
        }

        
        protected override IEnumerable<KeyValuePair<string, object>> WriteToJson(Vector3 value, JsonSerializer serializer)
        {
            return new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("x", value.x),
                new KeyValuePair<string, object>("y", value.y),
                new KeyValuePair<string, object>("z", value.z),
            };
        }
    }
}