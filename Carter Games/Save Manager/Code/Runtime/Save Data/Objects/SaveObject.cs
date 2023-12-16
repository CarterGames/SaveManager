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
using System.Linq;
using System.Reflection;
using CarterGames.Assets.SaveManager.Serializiation;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    [Serializable]
    public abstract class SaveObject : SaveManagerAsset
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        [SerializeField, HideInInspector] private bool isExpanded;  // Only used in the editor.
        [SerializeField, HideInInspector] private string saveKey;
        
        private Dictionary<string, SaveValueBase> lookupCache;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if the save object is initialized.
        /// </summary>
        public bool IsInitialized => !string.IsNullOrEmpty(saveKey);
        
        
        /// <summary>
        /// A lookup of all the save values in the object.
        /// </summary>
        public Dictionary<string, SaveValueBase> Lookup
        {
            get
            {
                if (lookupCache is { } && lookupCache.Count > 0) return lookupCache;
                lookupCache = GetSaveValue();
                return lookupCache;
            }
        }


        /// <summary>
        /// The save key of the object, implemented in inheritors only. 
        /// </summary>
        public string SaveKey => saveKey;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void Reset()
        {
            lookupCache ??= new Dictionary<string, SaveValueBase>();
            lookupCache?.Clear();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Save Object Handling
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initializes the object for use with the save.
        /// </summary>
        public void Initialize()
        {
            SaveManager.RegisterObject(this);
        }


        /// <summary>
        /// Sets the save key for this object.
        /// </summary>
        /// <remarks>NOTE: Only do this is you need to make variants at runtime. Otherwise you'll just bloat the save data with a load of duplicates.</remarks>
        /// <param name="value">The key to set to.</param>
        public void SetSaveKey(string value)
        {
            saveKey = value;
        }


        /// <summary>
        /// Saves the object to the save.
        /// </summary>
        public virtual void Save()
        {
            SaveManager.RegisterObject(this);
            SaveManager.UpdateAndSaveObject(this);
        }

        
        /// <summary>
        /// Loads the data to the object from the save.
        /// </summary>
        public virtual void Load()
        {
            if (!SaveManager.TryGetSaveValuesLookup(SaveKey, out var data)) return;

            if (data == null) return;

            var saveValues = GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(t => t.GetValue(this)).ToArray();
            
            foreach (var t in saveValues)
            {
                if (!t.GetType().IsSubclassOf(typeof(SaveValueBase))) continue;
                var sv = (SaveValueBase)t;
                if (!data.ContainsKey(sv.key)) continue;
                JsonUtility.FromJsonOverwrite(data[sv.key], sv);
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
        public virtual SerializableDictionary<string, SaveValueBase> GetSaveValue()
        {
            var dic = new SerializableDictionary<string, SaveValueBase>();

            
            var saveValues = GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(t => t.GetValue(this)).ToArray();

            
            for (var i = 0; i < saveValues.Length; i++)
            {
                if (!saveValues[i].GetType().IsSubclassOf(typeof(SaveValueBase))) continue;
                var sv = (SaveValueBase) saveValues[i];
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
            if (!SaveManager.TryResetElementFromSave(SaveKey, key)) return;

            if (!Lookup.ContainsKey(key)) return;
            Lookup[key].ResetValue();
        }
    }
}