/*
 * Copyright (c) 2024 Carter Games
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
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A generic for a save value.
    /// </summary>
    /// <typeparam name="T">The type for the save value.</typeparam>
    [Serializable]
    public class SaveValue<T> : SaveValueBase
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        [SerializeField] private T value;
        [SerializeField] private T defaultValue;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// The value for the save value.
        /// </summary>
        public T Value
        {
            get => (T) ValueObject;
            set => ValueObject = value;
        }
        
        
        /// <summary>
        /// The value that the save value is currently set to.
        /// </summary>
        public override object ValueObject
        {
            get => value;
            set
            {
                if (this.value == null)
                {
                    this.value = (T) value;
                }
                else if (!this.value.Equals(value))
                {
                    this.value = (T) value;
                }
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        /// <summary>
        /// Creates a new save value.
        /// </summary>
        public SaveValue()
        {
            key = Guid.NewGuid().ToString();
        }
        
        
        /// <summary>
        /// Creates a new save value with the key entered.
        /// </summary>
        /// <param name="key">The save key for the value.</param>
        public SaveValue(string key)
        {
            this.key = key;
        }
        

        /// <summary>
        /// Creates a new save value with the key & default value entered.
        /// </summary>
        /// <param name="key">The save key for the value.</param>
        /// <param name="value">The default value for the save value.</param>
        public SaveValue(string key, T value)
        {
            this.key = key;
            this.value = value;
            defaultValue = value;
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Initializes the save value for use.
        /// </summary>
        public void Initialize()
        {
            ValueObject = defaultValue;
        }
        
        
        /// <summary>
        /// Assigns the save value from the entered data.
        /// </summary>
        /// <param name="data">The data to read.</param>
        public override void AssignFromObject(SaveValueBase data)
        {
            key = data.key;
            value = (T) data.ValueObject;
        }
        
        
        /// <summary>
        /// Resets the save value to its default setup.
        /// </summary>
        public override void ResetValue(bool useDefault = true)
        {
            try
            {
                ValueObject = Activator.CreateInstance(typeof(T), useDefault ? defaultValue : default(T));
            }
#pragma warning disable 0168
            catch (Exception e)
            {
                // Reflection reset if ^ doesn't work.
                GetType().GetProperty("ValueObject", BindingFlags.Public | BindingFlags.Instance).SetValue(this, useDefault ? defaultValue : default(T));
            }
#pragma warning restore
        }
    }
}