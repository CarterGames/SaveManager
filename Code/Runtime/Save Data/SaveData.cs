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
using CarterGames.Assets.SaveManager.Serializiation;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// The save data that is actually saved.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Save Data", menuName = "Carter Games/Save Manager/Save Data", order = 4)]
    public sealed class SaveData : SaveManagerAsset
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private List<SaveObject> saveData;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// A list of all the save objects stored on this save data.
        /// </summary>
        public List<SaveObject> Data
        {
            get => saveData;
            set => saveData = value;
        }


        /// <summary>
        /// The serialized data that is saved.
        /// </summary>
        public SerializableDictionary<string, SerializableDictionary<string, string>> SerializableData
        {
            get
            {
                // List<string> = Dictionary<string, SaveValueBase>
                var items = new SerializableDictionary<string, SerializableDictionary<string, string>>();

                foreach (var saveValue in Data)
                {
                    var data = saveValue.GetSaveValue();

                    var converted = new SerializableDictionary<string, string>();

                    foreach (var v in data.Values)
                    {
                        converted.Add(v.key, JsonUtility.ToJson(v, AssetAccessor.GetAsset<SettingsAssetRuntime>().Prettify));
                    }
                    
                    items.Add(saveValue.SaveKey, converted);
                }

                return items;
            }
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initializes the save data for use.
        /// </summary>
        public void Initialize()
        {
            foreach (var saveValue in saveData)
            {
                SaveManager.RegisterObject(saveValue);
            }
        }
    }
}