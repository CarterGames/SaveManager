/*
 * Copyright (c) 2018-Present Carter Games
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
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Serializiation
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