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
using System.Linq;
using CarterGames.Assets.SaveManager.Slots;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Demo
{
    /// <summary>
    /// Handles the slot displays in the example scene.
    /// </summary>
    public sealed class ExampleSlotMenuDisplayManager : MonoBehaviour
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private Transform slotDisplayParent;
        [SerializeField] private GameObject slotDisplayPrefab;


        private Dictionary<int, ExampleSlotDisplay> displaysLookup;
        private List<int> spawnedIds = new List<int>();

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        // Gets the highest slot id made.
        private int HighestId
        {
            get
            {
                if (!spawnedIds.Any()) return 0;
                return spawnedIds.Max();
            }
        }


        // Gets any missed ids that are not created between the 1 > highest slot id.
        private IEnumerable<int> MissedIds
        {
            get
            {
                var list = new List<int>();

                for (var i = 1; i < HighestId; i++)
                {
                    if (spawnedIds.Contains(i)) continue;
                    list.Add(i);
                }

                return list;
            }
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnEnable()
        {
            displaysLookup = new Dictionary<int, ExampleSlotDisplay>();
            
            if (!SaveManager.IsLoading)
            {
                SpawnSlotObjects();
            }
            else
            {
                SaveManager.GameLoadedEvt.Add(SpawnSlotObjects);
            }
            
            SaveSlotManager.SlotCreatedEvt.Add(OnSlotCreated);
        }


        private void OnDisable()
        {
            SaveManager.GameLoadedEvt.Remove(SpawnSlotObjects);
            SaveSlotManager.SlotCreatedEvt.Remove(OnSlotCreated);
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Spawns the slot GUI's for the current slot data in the save.
        /// </summary>
        private void SpawnSlotObjects()
        {
            SaveManager.GameLoadedEvt.Remove(SpawnSlotObjects);

            var slotIndexesSpawned = new List<int>();
            
            foreach (var entry in SaveSlotManager.AllSlots)
            {
                var instance = Instantiate(slotDisplayPrefab, slotDisplayParent);
                var instanceDisplayScript = instance.GetComponentInChildren<ExampleSlotDisplay>();
                
                instanceDisplayScript.AssignSlot(entry.Value);
                slotIndexesSpawned.Add(entry.Key);
                displaysLookup.Add(entry.Key, instanceDisplayScript);
                
                spawnedIds.Add(entry.Key);
            }

            
            // Any missing in-between.
            foreach (var missed in MissedIds)
            {
                var instance = Instantiate(slotDisplayPrefab, slotDisplayParent);
                var instanceDisplayScript = instance.GetComponentInChildren<ExampleSlotDisplay>();
                
                instance.transform.SetSiblingIndex(missed - 1);
                instanceDisplayScript.SetSlotId(missed);
                instanceDisplayScript.UpdateDisplay();
                slotIndexesSpawned.Add(missed);
                displaysLookup.Add(missed, instanceDisplayScript);
            }
            
            
            // Extra slots if applicable.
            if (SaveSlotManager.TotalSlotsRestricted)
            {
                // Spawn all remaining slot indexes in the right place.
                // So if limited to x slots it'll show the rest as empty.
                for (var i = 0; i < SaveSlotManager.RestrictedSlotsTotal; i++)
                {
                    var adjusted = i + 1;
                    
                    // Skip if already spawned.
                    if (slotIndexesSpawned.Contains(adjusted)) continue;
                    
                    var instance = Instantiate(slotDisplayPrefab, slotDisplayParent);
                    var instanceDisplayScript = instance.GetComponentInChildren<ExampleSlotDisplay>();
                    
                    instanceDisplayScript.SetSlotId(adjusted);
                    instanceDisplayScript.UpdateDisplay();
                    instance.transform.SetSiblingIndex(adjusted);
                    displaysLookup.Add(adjusted, instanceDisplayScript);
                }
                
            }
            else
            {
                // Spawn an extra slot to allow new slots to be added.
                var instance = Instantiate(slotDisplayPrefab, slotDisplayParent);
                var instanceDisplayScript = instance.GetComponentInChildren<ExampleSlotDisplay>();
                    
                instanceDisplayScript.SetSlotId(instance.transform.GetSiblingIndex() + 1);
                instanceDisplayScript.UpdateDisplay();
                instance.transform.SetAsLastSibling();
                displaysLookup.Add(instance.transform.GetSiblingIndex() + 1, instanceDisplayScript);
            }
        }


        /// <summary>
        /// Runs when a slot is created to add extra slot displays to the GUI if possible.
        /// </summary>
        /// <param name="newSlot">The new slot made.</param>
        private void OnSlotCreated(SaveSlot newSlot)
        {
            if (displaysLookup.ContainsKey(newSlot.SlotId))
            {
                displaysLookup[newSlot.SlotId].AssignSlot(newSlot);
            }

            if (SaveSlotManager.TotalSlotsRestricted) return;
            if (newSlot.SlotId <= HighestId) return;
            
            // Spawn an extra slot to allow new slots to be added.
            var instance = Instantiate(slotDisplayPrefab, slotDisplayParent);
            var instanceDisplayScript = instance.GetComponentInChildren<ExampleSlotDisplay>();
                    
            instanceDisplayScript.UpdateDisplay();
            instance.transform.SetAsLastSibling();
            displaysLookup.Add(instance.transform.GetSiblingIndex() + 1, instanceDisplayScript);
        }
    }
}