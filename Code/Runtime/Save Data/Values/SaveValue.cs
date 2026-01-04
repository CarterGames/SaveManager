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


        /// <summary>
        /// Gets if there is a default value defined.
        /// </summary>
        public bool HasDefaultValue => hasDefaultValue;
        
        
        /// <summary>
        /// Gets the default value defined.
        /// </summary>
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


        /// <summary>
        /// Gets the type the save value is saving.
        /// </summary>
        [JsonIgnore]
        public override Type ValueType => typeof(T);
        

        /// <summary>
        /// Gets the default value object value.
        /// </summary>
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