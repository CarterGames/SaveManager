using System;
using System.Globalization;
using System.Linq;
using CarterGames.Assets.SaveManager.Slots;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    [Serializable]
    public class SaveSlot
    {
        private int slotIndex;
        private DateTime saveDate;
        private TimeSpan playtime;
        
        private JArray saveData;


        public int SlotIndex => slotIndex;
        public DateTime LastSaveDate => saveDate;
        public TimeSpan Playtime
        {
            get => playtime;
            private set => playtime = value;
        }

        public JArray GetDataArray => saveData;
        
        private static float PlaytimeStartTime { get; set; }



        public void ListenForSlotEvents()
        {
            SaveSlotManager.SlotLoadedEvt.Add(OnSlotLoaded);
            SaveSlotManager.SlotUnloadedEvt.Add(OnSlotUnloaded);
        }

        
        public static SaveSlot NewSlot(int index)
        {
            var valuesJson = new JArray();

            foreach (var entry in SaveObjectController.GetSlotSaveFields(index))
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
                slotIndex = index,
                saveDate = DateTime.UtcNow,
                saveData = valuesJson
            };
        }

        
        public JObject ToJsonObject()
        {
            return new JObject()
            {
                ["$slot_save_date"] = saveDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["$slot_playtime"] = playtime.TotalSeconds.ToString("N0"),
                ["$slot_data"] = saveData
            };
        }


        public void FromJsonObject(JToken data)
        {
            saveDate = data["$slot_save_date"].Value<DateTime>();
            playtime = TimeSpan.FromSeconds(data["$slot_playtime"].Value<double>());

            SmDebugLogger.LogDev($"Slot {slotIndex} loading from this data:\n{data["$slot_data"].Value<JArray>()}");
            
            saveData = data["$slot_data"].Value<JArray>();
        }


        public void UpdateData(JArray data)
        {
            saveData = data;
        }


        private void OnSlotLoaded(int index)
        {
            if (SlotIndex != index) return;
            PlaytimeStartTime = Time.time;
        }
        
        
        private void OnSlotUnloaded(int index)
        {
            if (SlotIndex != index) return;
            Playtime += TimeSpan.FromSeconds(Time.time - PlaytimeStartTime);
            saveDate = DateTime.UtcNow;
        }
    }
}