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