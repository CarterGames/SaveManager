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
using System.Reflection;
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Serializiation;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A Save Object that stores save values on it for the save system to use.
    /// </summary>
    [Serializable]
    public abstract class SaveObject : SmDataAsset
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        [SerializeField] [HideInInspector] private bool editor_isExpanded;  // Only used in the editor.
        [SerializeField] [HideInInspector] private bool editor_hasWarning;  // Only used in the editor.
        [SerializeField] [HideInInspector] private string editor_warningMessage = string.Empty;  // Only used in the editor.
        
        private Dictionary<string, SaveValueBase> lookupCache;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// A lookup of all the save values in the object.
        /// </summary>
        public Dictionary<string, SaveValueBase> Lookup
        {
            get
            {
                if (lookupCache is { } && lookupCache.Count > 0) return lookupCache;
                lookupCache = GetSaveValues();
                return lookupCache;
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private void OnEnable()
        {
            hideFlags = HideFlags.DontSave;     // Done so the asset doesn't persist in editor.
        }
        
        
        private void Reset()
        {
            lookupCache ??= new Dictionary<string, SaveValueBase>();
            lookupCache?.Clear();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Save Object Handling
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Loads the data to the object from the save.
        /// </summary>
        public virtual void Load(JArray saveData)
        {
            if (saveData == null) return;

            var saveValues = GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(t => t.GetValue(this)).ToArray();
            
            foreach (var t in saveValues)
            {
                if (t == null) continue;
                if (!t.GetType().IsSubclassOf(typeof(SaveValueBase))) continue;
                
                var sv = (SaveValueBase)t;
                
                // Checks for a null or empty key & skips it if it is not populated.
                if (string.IsNullOrEmpty(sv.key))
                {
                    SmDebugLogger.LogWarning(SaveManagerErrorCode.NoSaveKeyAssigned.GetErrorMessageFormat(sv));
                    continue;
                }
                
                // Loads the data to each save value.
                foreach (var entry in saveData)
                {
                    if (entry["$key"].Value<string>() != sv.key) continue;
                    sv.AssignFromJson(entry);
                }
            }
        }


        /// <summary>
        /// Resets the save values of all the save objects.
        /// </summary>
        public virtual void ResetObjectSaveValues()
        {
            foreach (var saveValue in Lookup.Values)
            {
                saveValue.ResetValue();
            }
            
            lookupCache?.Clear();
        }


        /// <summary>
        /// Gets the save values on this object.
        /// </summary>
        /// <returns>A dictionary as an object.</returns>
        public virtual SerializableDictionary<string, SaveValueBase> GetSaveValues()
        {
            var dic = new SerializableDictionary<string, SaveValueBase>();

            
            var saveValues = GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(t => t.GetValue(this)).ToArray();

            
            for (var i = 0; i < saveValues.Length; i++)
            {
                if (saveValues[i] == null) continue;
                if (!saveValues[i].GetType().IsSubclassOf(typeof(SaveValueBase))) continue;
                var sv = (SaveValueBase) saveValues[i];
                
                if (string.IsNullOrEmpty(sv.key)) continue;
                dic.Add(sv.key, sv);
            }
            
            
            return dic;
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Save Value Handling
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets if there is a save value of the requested key.
        /// </summary>
        /// <param name="key">The key to find.</param>
        /// <returns>If it exists.</returns>
        public bool HasValue(string key)
        {
            return Lookup.ContainsKey(key);
        }
        
        
        /// <summary>
        /// Gets the save value of the key entered. 
        /// </summary>
        /// <param name="key">The key to find.</param>
        /// <typeparam name="T">The type to cast the save value type to.</typeparam>
        /// <returns>The save value of the type requested or default if none found.</returns>
        public SaveValue<T> GetValue<T>(string key)
        {
            if (!Lookup.ContainsKey(key)) return default;
            var saveValue = (SaveValue<T>) Lookup[key];
            return saveValue;
        }


        /// <summary>
        /// Sets the value to the value entered.
        /// </summary>
        /// <param name="key">The key to edit.</param>
        /// <param name="value">The value to set to.</param>
        public void SetValue(string key, object value)
        {
            if (!Lookup.ContainsKey(key)) return;
            Lookup[key].ValueObject = value;
        }
        
        
        /// <summary>
        /// Resets a value with the corresponding key. 
        /// </summary>
        /// <param name="key">The key to find.</param>
        public void ResetElement(string key) 
        {
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Save Object Attributes
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Defines the save category of a save object. Cannot be applied to any other class except save objects.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class)]
        protected sealed class SaveCategoryAttribute : Attribute
        {
            /// <summary>
            /// The name of the category to use.
            /// </summary>
            public string Category;


            /// <summary>
            /// The order of the object in the category.
            /// </summary>
            public int OrderInCategory;
            
            
            /// <summary>
            /// Defines a save category with the entered value.
            /// </summary>
            /// <param name="category">The name of the category to use.</param>
            /// <param name="order">The order of the object in the category. Def = 0.</param>
            public SaveCategoryAttribute(string category, int order = 0)
            {
                Category = category;
                OrderInCategory = order;
            }
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Obsolete API
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [Obsolete("[Legacy API] Please use SaveManager.Save() instead.")]
        public virtual void Save() { }
    }
}