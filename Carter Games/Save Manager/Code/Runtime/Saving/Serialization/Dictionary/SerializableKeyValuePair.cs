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
    /// Defines a data structure for a key pair value that is serializable.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue> : IEquatable<SerializableKeyValuePair<TKey, TValue>>
    {
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Fields
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        [SerializeField] public TKey key;
        [SerializeField] public TValue value;

        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Properties
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// The key for the entry.
        /// </summary>
        public TKey Key => key;
        
        
        /// <summary>
        /// The value for the entry.
        /// </summary>
        public TValue Value
        {
            get => value;
            set => this.value = value;
        }

        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Constructors
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// Makes a new blank key value pair.
        /// </summary>
        public SerializableKeyValuePair()
        { }
        
        
        /// <summary>
        /// Makes a new blank key value pair with the data entered.
        /// </summary>
        /// <param name="key">The key to set.</param>
        /// <param name="value">The value to set.</param>
        public SerializableKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
        
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Operators
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// Implicitly converts a normal version to the this type.
        /// </summary>
        /// <param name="pair">The pair to convert.</param>
        /// <returns>A new instance of this type with the pair data entered.</returns>
        public static implicit operator SerializableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
        {
            return new SerializableKeyValuePair<TKey, TValue>(pair.Key, pair.Value);
        }
        
        
        /// <summary>
        /// Implicitly converts a this type to the normal version.
        /// </summary>
        /// <param name="pair">The pair to convert.</param>
        /// <returns>A new instance of the standard type with the pair data entered.</returns>
        public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> pair)
        {
            return new KeyValuePair<TKey, TValue>(pair.key, pair.value);
        }

        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   IEquatable
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// defines the equals check for the class.
        /// </summary>
        /// <param name="other">the other class to compare to.</param>
        /// <returns>The result of the comparision.</returns>
        public bool Equals(SerializableKeyValuePair<TKey, TValue> other)
        {
            var comparer1 = EqualityComparer<TKey>.Default;
            var comparer2 = EqualityComparer<TValue>.Default;

            return comparer1.Equals(key, other.key) && comparer2.Equals(value, other.value);
        }

        
        /// <summary>
        /// Overrides the hash code setup for this class.
        /// </summary>
        /// <returns>The hash code for the class.</returns>
        public override int GetHashCode()
        {
            var comparer1 = EqualityComparer<TKey>.Default;
            var comparer2 = EqualityComparer<TValue>.Default;

            int h0;
            h0 = comparer1.GetHashCode(key);
            h0 = (h0 << 5) + h0 ^ comparer2.GetHashCode(value);
            return h0;
        }

        
        /// <summary>
        /// Overrides the ToString conversion for the class.
        /// </summary>
        /// <returns>The string format for the class.</returns>
        public override string ToString()
        {
            return $"(Key: {key}, Value: {value})";
        }
    }
}