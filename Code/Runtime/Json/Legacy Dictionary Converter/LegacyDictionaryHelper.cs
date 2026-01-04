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
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A helper class to handle converting older SM 2.x saves where the Serializable Dictionary type was used to port over correctly to the 3.X format.
    /// </summary>
    public static class LegacyDictionaryHelper 
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Converts a old dictionary json setup to the new 3.x setup.
        /// </summary>
        /// <param name="tokenValue">The value to read.</param>
        /// <returns>The converted value.</returns>
        public static JToken ConvertAnyLegacyDictionaries(JToken tokenValue)
        {
            if (!TryConvertAllDictionaries(tokenValue, out var entries)) return tokenValue;
            return entries;
        }


        /// <summary>
        /// Tries to convert all dictionary setups in the entered token and its children.
        /// </summary>
        /// <param name="token">The token to read.</param>
        /// <param name="updated">The updated token to return.</param>
        /// <returns>If it was successful.</returns>
        private static bool TryConvertAllDictionaries(JToken token, out JObject updated)
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
                        jo.ReplaceDataAtPath(entry.Parent.Parent.Path, ConvertLegacyDictionaries(entry.Value<JArray>()));
                    }
                }
            }
        }


        /// <summary>
        /// Converts a legacy dictionary
        /// </summary>
        /// <param name="data">The data to convert.</param>
        /// <returns>The converted data.</returns>
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


        /// <summary>
        /// Updates a data at a path.
        /// </summary>
        /// <param name="root">The root token</param>
        /// <param name="path">The path for the token.</param>
        /// <param name="newValue">The new value to set.</param>
        /// <typeparam name="T">The type to apply.</typeparam>
        /// <returns>The updated object.</returns>
        private static JObject ReplaceDataAtPath<T>(this JToken root, string path, T newValue)
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