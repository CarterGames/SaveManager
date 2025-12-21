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
using System.Linq;
using CarterGames.Assets.SaveManager.NewtonsoftJson_Converters;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json;

namespace CarterGames.Assets.SaveManager
{
    public static class JsonHelper
    {
        private static Dictionary<Type, IAssetJsonConverter> cacheConvertersLookup;
        private static JsonSerializerSettings cacheSerializerSettings;
        private static JsonSerializer cacheSerializer;
 
        
        private static Dictionary<Type, IAssetJsonConverter> ConvertersLookup
        {
            get
            {
                if (cacheConvertersLookup != null)
                {
                    if (cacheConvertersLookup.Count > 0) return cacheConvertersLookup;
                }
        
                cacheConvertersLookup = new Dictionary<Type, IAssetJsonConverter>();
        
                foreach (var entry in AssemblyHelper.GetClassesOfType<IAssetJsonConverter>(false))
                {
                    cacheConvertersLookup.Add(entry.TargetType, entry);
                }
        
                return cacheConvertersLookup;
            }
        }


        public static JsonSerializerSettings SaveManagerSerializerSettings
        {
            get
            {
                if (cacheSerializerSettings != null) return cacheSerializerSettings;
                cacheSerializerSettings = new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    ContractResolver = new FieldsOnlyResolver()
                };

                foreach (var entry in ConvertersLookup.Values)
                {
                    cacheSerializerSettings.Converters.Add(entry.Converter);
                }

                return cacheSerializerSettings;
            }
        }
        

        public static JsonSerializer SaveManagerSerializer
        {
            get
            {
                if (cacheSerializer != null) return cacheSerializer;
                cacheSerializer = JsonSerializer.Create(SaveManagerSerializerSettings);
                return cacheSerializer;
            }
        }


        public static IEnumerable<JsonConverter> SaveManagerConverters =>
            ConvertersLookup.Values.Select(t => t.Converter);
    }
}