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
using System.Linq;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A helper class for custom JSON bits.
    /// </summary>
    public static class JsonHelper
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static Dictionary<Type, IAssetJsonConverter> cacheConvertersLookup;
        private static JsonSerializerSettings cacheSerializerSettings;
        private static JsonSerializer cacheSerializer;
 
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The converters in the asset to use.
        /// </summary>
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


        /// <summary>
        /// Gets the settings for the Save Manager JsonSerializer
        /// </summary>
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
        

        /// <summary>
        /// Gets the JsonSerializer the Save Manager uses for the game save.
        /// </summary>
        public static JsonSerializer SaveManagerSerializer
        {
            get
            {
                if (cacheSerializer != null) return cacheSerializer;
                cacheSerializer = JsonSerializer.Create(SaveManagerSerializerSettings);
                return cacheSerializer;
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the Save Managers json converters to use.
        /// </summary>
        public static IEnumerable<JsonConverter> SaveManagerConverters =>
            ConvertersLookup.Values.Select(t => t.Converter);
    }
}