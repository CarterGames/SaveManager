/*
 * Save Manager
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
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A resolver to get all serializable fields when saving/loading a class.
    /// </summary>
    public class FieldsOnlyResolver : DefaultContractResolver
    {
        /// <summary>
        /// Overrides the creation of properties for a JObject to avoid ones the asset shouldn't do anything with.
        /// </summary>
        /// <param name="type">The type to create from.</param>
        /// <param name="memberSerialization">The target member serialization setting.</param>
        /// <returns>IList of JsonProperty</returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            var valid = ValidMembers(type);

            foreach (var entry in properties)
            {
                if (valid.All(t => t.Name != entry.PropertyName)) continue;
                
                entry.Readable = true;
                entry.Writable = true;
                entry.Ignored = false;
            }

            return properties;
        }


        /// <summary>
        /// Gets all the members to use.
        /// </summary>
        /// <param name="objectType">The type of the object targeting.</param>
        /// <returns>List of MemberInfo</returns>
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            return ValidMembers(objectType).ToList();
        }


        /// <summary>
        /// Gets the valid members to serialize for the Save Manager specifically.
        /// </summary>
        /// <remarks>
        /// Only intended to get public or [SerializeField] private fields. No properties that would normally be gotten.
        /// </remarks>
        /// <param name="objectType">The type of the object targeting.</param>
        /// <returns>IEnumerable of MemberInfo</returns>
        private IEnumerable<MemberInfo> ValidMembers(Type objectType)
        {
            return objectType
                .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(t => objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Contains(t));
        }
    }
}