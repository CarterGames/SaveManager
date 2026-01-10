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
using CarterGames.Assets.SaveManager.Slots;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A save slot in the save system.
    /// </summary>
    [Serializable]
    public class SaveSlot
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private ISlotMetaData[] slotMetaData;
        private int slotId;
        private DateTime saveDate;
        private TimeSpan playtime;
        private JArray saveData;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the slot id for use.
        /// </summary>
        public int SlotId => slotId;
        
        
        /// <summary>
        /// Gets the last time the slot was saved/changed at.
        /// </summary>
        public DateTime LastSaveDate => saveDate;
        
        
        /// <summary>
        /// Gets the total playtime with this slot active.
        /// </summary>
        public TimeSpan Playtime
        {
            get => playtime;
            private set => playtime = value;
        }

        
        /// <summary>
        /// Gets the data stores in this slot for use.
        /// </summary>
        public JArray GetDataArray => saveData;
        
        
        /// <summary>
        /// Gets the start time for the current play session.
        /// </summary>
        private static float PlaytimeStartTime { get; set; }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Makes a new save slot
        /// </summary>
        /// <param name="id">The id to set the slot as.</param>
        /// <returns>The generated slot for instance use.</returns>
        public static SaveSlot NewSlot(int id)
        {
            var valuesJson = new JArray();

            foreach (var entry in SaveObjectController.GetSlotSaveFields(id))
            {
                if (string.IsNullOrEmpty(entry.key))
                {
                    // Error, no save key defined for save value.
                    SmDebugLogger.LogWarning(SaveManagerErrorCode.NoSaveKeyAssigned.GetErrorMessageFormat(entry));
                    Debug.LogWarning($"Cannot save a value on {entry} as it doesn't have a save key assigned to it.");
                    continue;
                }
                    
                var defObj = JObject.FromObject(entry, JsonHelper.SaveManagerSerializer);
                var obj = new JObject
                {
                    ["$key"] = entry.key,
                    ["$value"] = defObj["value"],
                    ["$type"] = entry.ValueType.ToString()
                };

                if (defObj.SelectToken("hasDefaultValue") != null)
                {
                    if (defObj.SelectToken("hasDefaultValue").Value<bool>())
                    {
                        obj["$default"] = defObj["defaultValue"];
                    }
                }
                
                valuesJson.Add(obj);
            }

            return new SaveSlot()
            {
                slotId = id,
                saveDate = DateTime.UtcNow,
                saveData = valuesJson
            };
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Sets up the slot to listen for save slot manager events.
        /// </summary>
        public void ListenForSlotEvents()
        {
            SaveSlotManager.SlotLoadedEvt.Add(OnSlotLoaded);
            SaveSlotManager.SlotUnloadedEvt.Add(OnSlotUnloaded);
        }
        
        
        /// <summary>
        /// Converts the slot data into JSON for saving.
        /// </summary>
        /// <returns></returns>
        public JObject ToJsonObject()
        {
            return new JObject()
            {
                ["$slot_save_date"] = saveDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["$slot_playtime"] = playtime.TotalSeconds.ToString("N0"),
                ["$slot_data"] = saveData
            };
        }


        /// <summary>
        /// Loads the slot data from a provided JSON blob.
        /// </summary>
        /// <param name="data">The json to load from.</param>
        public void FromJsonObject(JToken data)
        {
            SmDebugLogger.LogDev($"Slot {slotId} loading from this data:\n{data["$slot_data"].Value<JArray>()}");
            
            saveDate = data["$slot_save_date"].Value<DateTime>();
            playtime = TimeSpan.FromSeconds(data["$slot_playtime"].Value<double>());
            saveData = data["$slot_data"].Value<JArray>();
        }


        /// <summary>
        /// Updates the data in the slot to the entered data.
        /// </summary>
        /// <param name="data">The data to set.</param>
        public void UpdateData(JArray data)
        {
            saveData = data;
        }


        /// <summary>
        /// Runs when the slot is loaded in the <see cref="SaveSlotManager"/>
        /// </summary>
        /// <param name="id">The slot id loaded.</param>
        private void OnSlotLoaded(int id)
        {
            if (SlotId != id) return;
            PlaytimeStartTime = Time.time;
        }
        
        
        /// <summary>
        /// Runs when the slot is unloaded from the <see cref="SaveSlotManager"/>
        /// </summary>
        /// <param name="id">The slot id unloaded.</param>
        private void OnSlotUnloaded(int id)
        {
            if (SlotId != id) return;
            Playtime += TimeSpan.FromSeconds(Time.time - PlaytimeStartTime);
            saveDate = DateTime.UtcNow;
        }
    }
}