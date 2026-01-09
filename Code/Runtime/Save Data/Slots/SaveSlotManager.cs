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

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CarterGames.Shared.SaveManager;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Slots
{
    /// <summary>
    /// Handles all API around the save slots setup.
    /// </summary>
    public static class SaveSlotManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static Dictionary<int, SaveSlot> saveSlotData = new Dictionary<int, SaveSlot>();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if save slots are enabled.
        /// </summary>
        public static bool SlotsEnabled => SmAssetAccessor.GetAsset<DataAssetSettings>().UseSaveSlots;
        
        
        /// <summary>
        /// Gets if the system has loaded a slot.
        /// </summary>
        public static bool HasLoadedSlot => ActiveSlotIndex >= 0;
        
        
        /// <summary>
        /// Gets the total number of slots in use.
        /// </summary>
        public static int TotalSlotsInUse { get; private set; }
        
        
        /// <summary>
        /// Gets the active slot index if one is loaded.
        /// </summary>
        public static int ActiveSlotIndex { get; private set; } = -1;
        
        
        /// <summary>
        /// Gets if any slots have been defined in the save.
        /// </summary>
        public static bool HasAnySlots => TotalSlotsInUse > 0;
        
        
        /// <summary>
        /// Gets the active slot to read from.
        /// </summary>
        public static SaveSlot ActiveSlot { get; private set; }
        
        
        /// <summary>
        /// Gets the last active slot if applicable.
        /// </summary>
        public static SaveSlot LastActiveSlot { get; private set; }
        

        /// <summary>
        /// Gets all the slots in the system in a read-only state.
        /// </summary>
        public static IReadOnlyDictionary<int, SaveSlot> AllSlots => saveSlotData;
        
        
        /// <summary>
        /// Gets if the number of slots in the save manager are restricted to a cap.
        /// </summary>
        public static bool TotalSlotsRestricted => SmAssetAccessor.GetAsset<DataAssetSettings>().LimitAvailableSlots;
        
        
        /// <summary>
        /// Gets the max slots the user can have. Only applies if TotalSlotsRestricted is true.
        /// </summary>
        public static int RestrictedSlotsTotal => SmAssetAccessor.GetAsset<DataAssetSettings>().LimitAvailableSlots 
            ? SmAssetAccessor.GetAsset<DataAssetSettings>().MaxUserSaveSlots 
            : -1;

        
        /// <summary>
        /// Raised when a slot is created by the user.
        /// </summary>
        public static readonly Evt<SaveSlot> SlotCreatedEvt = new Evt<SaveSlot>();
        
        
        /// <summary>
        /// Raised when a slot is deleted by the user.
        /// </summary>
        public static readonly Evt<int> SlotDeletedEvt = new Evt<int>();
        
        
        /// <summary>
        /// Raised when a loaded slot is unloaded.
        /// </summary>
        public static readonly Evt<int> SlotUnloadedEvt = new Evt<int>();
        
        
        /// <summary>
        /// Raised when a slot is loaded.
        /// </summary>
        public static readonly Evt<int> SlotLoadedEvt = new Evt<int>();
        
        
        /// <summary>
        /// Raised when loading a slot fails for any reason.
        /// </summary>
        public static readonly Evt<int> SlotLoadFailedEvt = new Evt<int>();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Internal System Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Intended for internal use only!
        /// DO NOT call this method yourself!
        /// - Loads the slot data from the save into the setup for use.
        /// - Is automatically called by the Save Manager on successful load of a save data.
        /// </summary>
        /// <param name="data">The data to load from.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void INTERNAL_LoadSlotDataFromSave(JToken data)
        {
            TotalSlotsInUse = (int) data["$content"]["$slots"]["$settings"]["$slots_defined"];
            ActiveSlotIndex = (int) data["$content"]["$slots"]["$settings"]["$slots_last_active"];

            saveSlotData = new Dictionary<int, SaveSlot>();
            
            if (TotalSlotsInUse > 0)
            {
                var totalParsed = 0;
                var i = 0;
                
                while (totalParsed < TotalSlotsInUse)
                {
                    i++;
                    
                    if (data["$content"]["$slots"].SelectToken($"$slot_{i}") == null) continue;
                    
                    var slot = SaveSlot.NewSlot(i);
                    slot.FromJsonObject(data["$content"]["$slots"][$"$slot_{i}"]);
                    slot.ListenForSlotEvents();
                    saveSlotData.Add(i, slot);
                    
                    totalParsed++;
                }
                
                SaveObjectController.InitializeSlotObjects(saveSlotData.Select(t => t.Value));

                foreach (var entry in saveSlotData)
                {
                    foreach (var SlotSaveObject in SaveObjectController.AllSlotSaveObjects)
                    {
                        foreach (var saveObject in SlotSaveObject.Value.SlotSaveObjects)
                        {
                            saveObject.Load(entry.Value.GetDataArray);
                        }
                    }
                }
            
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
            else
            {
                SmDebugLogger.Log(SaveManagerErrorCode.NoSlotsToLoad.GetErrorMessageFormat());
            }
        }
        
        
        /// <summary>
        /// Intended for internal use only!
        /// DO NOT call this method yourself!
        /// - Gets the save data to save for the users save slots.
        /// - Is automatically called by the Save Manager on saving the game.
        /// </summary>
        /// <returns>A JObject of the data to save for slots.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static JObject INTERNAL_GetSlotDataToSave()
        {
            var structure = new JObject
            {
                ["$settings"] = new JObject
                {
                    ["$slots_available"] = SlotsEnabled,
                    ["$slots_defined"] = TotalSlotsInUse,
                    ["$slots_last_active"] = ActiveSlotIndex,
                }
            };

            if (saveSlotData.Count <= 0)
            {
                return structure;
            }
            
            foreach (var slotData in saveSlotData)
            {
                // Save the current data from the slot loaded bits, so its up to date.
                var valuesJson = new JArray();
                
                foreach (var entry in SaveObjectController.GetSlotSaveFields(slotData.Key))
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
                
                slotData.Value.UpdateData(valuesJson);
                structure[$"$slot_{slotData.Key}"] = slotData.Value.ToJsonObject();
            }

            return structure;
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Public API Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Tries to create a save slot at the desired index if possible
        /// </summary>
        /// <param name="slotId">The slot index to create for.</param>
        /// <param name="newSlot">The new slot made if successful.</param>
        /// <returns>If it was successful (bool).</returns>
        public static bool TryCreateSlotAtIndex(int slotId, out SaveSlot newSlot)
        {
            saveSlotData ??= new Dictionary<int, SaveSlot>();

            if (saveSlotData.ContainsKey(slotId))
            {
                newSlot = null;
                SmDebugLogger.Log(SaveManagerErrorCode.SlotIdAlreadyExists.GetErrorMessageFormat());
                return false;
            }
            
            if (TotalSlotsRestricted)
            {
                if (slotId > RestrictedSlotsTotal)
                {
                    // Failed as slot cap hit.
                    newSlot = null;
                    SmDebugLogger.Log(SaveManagerErrorCode.SlotCapReached.GetErrorMessageFormat());
                    return false;
                }
            }
            
            newSlot = SaveSlot.NewSlot(slotId);
            
            saveSlotData.Add(slotId, newSlot);
            SaveObjectController.InitializeNewSaveSlot(newSlot);
            TotalSlotsInUse++;
            
            SaveManager.SaveGame();
            SlotCreatedEvt.Raise(newSlot);
            return true;
        }
        
        
        /// <summary>
        /// Tries to create a new slot at the next available index.
        /// </summary>
        /// <param name="newSlot">The new slot made if successful.</param>
        /// <returns>If it was successful (bool).</returns>
        public static bool TryCreateSlot(out SaveSlot newSlot)
        {
            saveSlotData ??= new Dictionary<int, SaveSlot>();
            var slotIndex = saveSlotData.Count + 1;
            
            if (TotalSlotsRestricted)
            {
                if (slotIndex > RestrictedSlotsTotal)
                {
                    // Failed as slot cap hit.
                    newSlot = null;
                    SmDebugLogger.Log(SaveManagerErrorCode.SlotCapReached.GetErrorMessageFormat());
                    return false;
                }
            }
            
            newSlot = SaveSlot.NewSlot(slotIndex);
            
            saveSlotData.Add(slotIndex, newSlot);
            SaveObjectController.InitializeNewSaveSlot(newSlot);
            TotalSlotsInUse++;
            
            SaveManager.SaveGame();
            SlotCreatedEvt.Raise(newSlot);
            return true;
        }


        /// <summary>
        /// Unloads the current slot if a slot if currently loaded.
        /// </summary>
        public static void UnloadCurrentSlot()
        {
            if (!HasLoadedSlot) return;

            LastActiveSlot = saveSlotData[ActiveSlotIndex];
            
            ActiveSlotIndex = -1;
            ActiveSlot = null;
            
            SlotUnloadedEvt.Raise(LastActiveSlot.SlotId);
        }
        
        
        /// <summary>
        /// Loads the slot requested when called if it isn't already.
        /// </summary>
        /// <param name="slotIndex">The slot to load.</param>
        public static void LoadSlot(int slotIndex)
        {
            if (HasLoadedSlot)
            {
                UnloadCurrentSlot();
            }

            if (!saveSlotData.ContainsKey(slotIndex))
            {
                SlotLoadFailedEvt.Raise(slotIndex);
                return;
            }
            
            ActiveSlotIndex = slotIndex;
            ActiveSlot = saveSlotData[ActiveSlotIndex];
            
            SlotLoadedEvt.Raise(ActiveSlot.SlotId);
        }


        /// <summary>
        /// Deletes a slot id from the system when called.
        /// Note: This cannot be undone once executed!
        /// </summary>
        /// <param name="slotId">The slot to remove.</param>
        public static void DeleteSlot(int slotId)
        {
            if (!saveSlotData.ContainsKey(slotId)) return;
            
            saveSlotData.Remove(slotId);
            SaveObjectController.RemoveSlotObjects(slotId);
            TotalSlotsInUse--;
            
            SaveManager.SaveGame();
            SlotDeletedEvt.Raise(slotId);
        }
    }
}