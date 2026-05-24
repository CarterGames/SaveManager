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
using UnityEngine;

namespace CarterGames.Shared.SaveManager.Serializiation
{
    /// <summary>
    /// A custom dictionary class to store a serializable version of a dictionaries data.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Fields
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> list = new List<SerializableKeyValuePair<TKey, TValue>>();

        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   ISerializationCallbackReceiver Implementation
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */

        /// <summary>
        /// Runs before the class is serialized.
        /// </summary>
        public void OnBeforeSerialize()
        {
            if (list.Count > Count)
            {
                AddNewValue();
            }
            else
            {
                UpdateSerializedValues();
            }
        }
        
        
        /// <summary>
        /// Runs after the class is deserialized.
        /// </summary>
        public void OnAfterDeserialize()
        {
            Clear();

            for (var i = 0; list != null && i < list.Count; i++)
            {
                var current = list[i];
                
#if UNITY_2021_1_OR_NEWER
                if (current.key != null)
                {
                    TryAdd(current.key, current.value);
                }
#elif UNITY_2020
                if (current.key != null)
                {
                    if (ContainsKey(current.key)) continue;
                    Add(current.key, current.value);
                }
#endif
            }
        }
        
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Methods
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// Updates the list when called.
        /// </summary>
        private void UpdateSerializedValues()
        {
            list.Clear();
            
            foreach(var pair in this)
            { 
                list.Add(pair);
            }
        }

        
        /// <summary>
        /// Adds a new value when called
        /// </summary>
        private void AddNewValue()
        {
#if UNITY_2021_1_OR_NEWER
            var current = list[^1];
            
            if (current.key != null)
            {
                TryAdd(current.Key, current.value);
            }
#elif UNITY_2020
            var current = list[list.Count - 1];
            
            if (current.key != null)
            {
                if (ContainsKey(current.key)) return;
                Add(current.key, current.value);
            }
#endif
        }
    }
}