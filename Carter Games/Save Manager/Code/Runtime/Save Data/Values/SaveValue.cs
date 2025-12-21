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
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A generic for a save value.
    /// </summary>
    /// <typeparam name="T">The type for the save value.</typeparam>
    [Serializable]
    [JsonObject]
    public class SaveValue<T> : SaveValueBase
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        [SerializeField] private T value;
        [SerializeField] private bool hasDefaultValue;
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


        public bool HasDefaultValue => hasDefaultValue;
        
        
        public T DefaultValue
        {
            get => (T) DefaultValueObject;
            set => DefaultValueObject = value;
        }
        
        
        /// <summary>
        /// The value that the save value is currently set to.
        /// </summary>
        [JsonIgnore] public override object ValueObject
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


        [JsonIgnore]
        public override Type ValueType => typeof(T);
        

        [JsonIgnore] protected override object DefaultValueObject
        {
            get => defaultValue;
            set
            {
                if (this.defaultValue == null)
                {
                    this.defaultValue = (T) value;
                }
                else if (!this.defaultValue.Equals(value))
                {
                    this.defaultValue = (T) value;
                }
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        /// <summary>
        /// Creates a new save value with the key entered.
        /// </summary>
        /// <param name="key">The save key for the value.</param>
        public SaveValue(string key)
        {
            this.key = key;
            hasDefaultValue = false;
        }
        

        /// <summary>
        /// Creates a new save value with the key and default value entered.
        /// </summary>
        /// <param name="key">The save key for the value.</param>
        /// <param name="defaultValue">The default value for the save value.</param>
        public SaveValue(string key, T defaultValue)
        {
            this.key = key;
            hasDefaultValue = true;
            this.defaultValue = defaultValue;
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Assign the save value from json
        /// </summary>
        /// <param name="jsonValue">The json value to assign from.</param>
        public override void AssignFromJson(JToken jsonValue)
        {
            key = jsonValue["$key"].Value<string>();

            if (jsonValue["$type"].Value<string>() != ValueType.ToString())
            {
                SmDebugLogger.LogWarning(SaveManagerErrorCode.SaveValueTypeMismatch.GetErrorMessageFormat(key, jsonValue["$type"].Value<string>(), ValueType.ToString()));
                return;
            }
            
            try
            {
                Value = jsonValue["$value"].ToObject<T>(JsonHelper.SaveManagerSerializer);

                if (jsonValue["$default"] == null) return;
                hasDefaultValue = true;
                DefaultValue = jsonValue["$default"].ToObject<T>(JsonHelper.SaveManagerSerializer);
            }
            catch (Exception e)
            {
                SmDebugLogger.LogWarning(SaveManagerErrorCode.SaveValueLoadFailed.GetErrorMessageFormat(key, e.Message, e.StackTrace));
            }
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
                try
                {
                    // Reflection reset if ^ doesn't work.
                    GetType().GetProperty("ValueObject", BindingFlags.Public | BindingFlags.Instance).SetValue(this, useDefault ? defaultValue : default(T));
                }
                catch (Exception exception)
                {
                    SmDebugLogger.LogWarning(SaveManagerErrorCode.SaveValueResetFailed.GetErrorMessageFormat(key, exception.Message, exception.StackTrace));
                }
            }
#pragma warning restore
        }
    }
}