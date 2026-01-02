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
using System.Collections.Generic;
using System.Linq;
using CarterGames.Assets.SaveManager.Slots;
using CarterGames.Shared.SaveManager;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles save object initialization and caching.
    /// </summary>
    public static class SaveObjectController
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets if the controller is initialized or not.
        /// </summary>
        public static bool IsInitialized { get; private set; }
        
        
        /// <summary>
        /// Gets if the slots save objects are initialized or not.
        /// </summary>
        public static bool AreSlotsInitialized { get; private set; }

        
        /// <summary>
        /// Get if there are any global save objects setup.
        /// </summary>
        public static bool HasGlobalSaveObjects
        {
            get
            {
                if (!IsInitialized) return false;
                return AllGlobalSaveObjects.Count > 0;
            }
        }
        
        
        /// <summary>
        /// Gets all the global save objects generated.
        /// </summary>
        private static List<SaveObject> AllGlobalSaveObjects { get; set; }
        
        
        /// <summary>
        /// Gets all the slot save objects generated.
        /// </summary>
        public static Dictionary<SaveSlot, SlotSaveObjectsContainer> AllSlotSaveObjects { get; private set; }
        
        
        /// <summary>
        /// Gets a lookup of all the global save values generated.
        /// </summary>
        public static Dictionary<SaveObject, List<SaveValueBase>> AllGlobalSaveValues { get; private set; }
        
        
        /// <summary>
        /// Gets a lookup of all the slot save values generated.
        /// </summary>
        public static Dictionary<SaveSlot, Dictionary<SaveObject, List<SaveValueBase>>> AllSlotSaveValues { get; private set; }
        
        
        /// <summary>
        /// Gets a collection of all the global save object types.
        /// </summary>
        public static IEnumerable<Type> GlobalSaveObjectTypes { get; private set; }
        
        
        /// <summary>
        /// Gets a collection of all the slot save object types.
        /// </summary>
        public static IEnumerable<Type> SlotSaveObjectTypes { get; private set; }
        
        
        /// <summary>
        /// Gets all the global save objects 
        /// </summary>
        public static IEnumerable<SaveObject> GlobalSaveObjects
        {
            get
            {
                if (!IsInitialized) return new List<SaveObject>();
                return AllGlobalSaveObjects;
            }
        }
        
        
        /// <summary>
        /// Gets if there are any slot save objects generated.
        /// </summary>
        public static bool HasSlotSaveObjects
        {
            get
            {
                if (!AreSlotsInitialized) return false;
                return AllSlotSaveObjects.Count > 0;
            }
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Rised when the controller is initialized.
        /// </summary>
        public static readonly Evt InitializedEvt = new Evt();
        
        
        /// <summary>
        /// Raised when all the slot save objects are initialized.
        /// </summary>
        public static readonly Evt SlotsInitializedEvt = new Evt();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initializes the controller to spawn all save objects etc.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Initialize()
        {
            if (IsInitialized) return;
            
            AllGlobalSaveObjects = new List<SaveObject>();
            AllGlobalSaveValues = new Dictionary<SaveObject, List<SaveValueBase>>();

            var objTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && typeof(SaveObject).IsAssignableFrom(x) && !x.IsAbstract && x.FullName != typeof(SaveObject).FullName)
                .Select(type => (SaveObject)ScriptableObject.CreateInstance(type))
                .Select(t => t.GetType())
                .ToArray(); 

            GlobalSaveObjectTypes = objTypes
                .Where(t => !t.IsAbstract && !t.IsSubclassOf(typeof(SlotSaveObject)))
                .ToArray();
            
            SlotSaveObjectTypes = objTypes
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(SlotSaveObject)))
                .ToArray();
            
            SpawnAndCacheGlobalSaveObjects();

            IsInitialized = true;
            InitializedEvt.Raise();
        }
        
        
        /// <summary>
        /// Initializes the slot save objects.
        /// </summary>
        /// <param name="slots">The slots to initialize.</param>
        public static void InitializeSlotObjects(IEnumerable<SaveSlot> slots)
        {
            if (AreSlotsInitialized) return;
            AllSlotSaveObjects = new Dictionary<SaveSlot, SlotSaveObjectsContainer>();

            foreach (var entry in slots)
            {
                if (AllSlotSaveObjects.ContainsKey(entry)) continue;
                AllSlotSaveObjects.Add(entry, new SlotSaveObjectsContainer());
            }

            CacheSlotSaveValues();

            AreSlotsInitialized = true;
            SlotsInitializedEvt.Raise();
        }
        
        
        /// <summary>
        /// Gets all the slot save objects for a particular slot.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static IEnumerable<SaveObject> SlotSaveObjects(int index)
        {
            if (!AreSlotsInitialized) return new List<SaveObject>();

            return AllSlotSaveObjects
                .Where(t=> t.Key.SlotId == index)
                .Select(t => t.Value)
                .SelectMany(x => x.SlotSaveObjects);
        }


        /// <summary>
        /// Spawns and caches the global save objects.
        /// </summary>
        private static void SpawnAndCacheGlobalSaveObjects()
        {
            // Spawn
            foreach (var objType in GlobalSaveObjectTypes)
            {
                AllGlobalSaveObjects.Add((SaveObject)ScriptableObject.CreateInstance(objType));
            }
            
            // Cache
            foreach (var entry in AllGlobalSaveObjects)
            {
                AllGlobalSaveValues.Add(entry, entry.GetSaveValues().Values.ToList());
            }
        }
        
        
        /// <summary>
        /// Caches all slot save objects.
        /// </summary>
        private static void CacheSlotSaveValues()
        {
            AllSlotSaveValues = new Dictionary<SaveSlot, Dictionary<SaveObject, List<SaveValueBase>>>();
            
            foreach (var entry in AllSlotSaveObjects)
            {
                var dic = new Dictionary<SaveObject, List<SaveValueBase>>();
                
                foreach (var obj in entry.Value.SlotSaveObjects)
                {
                    dic.Add(obj, obj.GetSaveValues().Values.ToList());
                }
                
                AllSlotSaveValues.Add(entry.Key, dic);
            }
        }
        
        
        /// <summary>
        /// Gets all the global save values with easier access than <see cref="AllGlobalSaveValues"/>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SaveValueBase> GetGlobalSaveFields()
        {
            return AllGlobalSaveValues
                .Where(t => GlobalSaveObjects.Contains(t.Key))
                .Where(t => t.Value.Any())
                .SelectMany(t => t.Value);
        }
        
        
        /// <summary>
        /// Gets all the slot save values with easier access than <see cref="SlotSaveObjects"/>
        /// </summary>
        /// <param name="slotId">The slot to get from</param>
        /// <returns>The save values for the desired slot.</returns>
        public static IEnumerable<SaveValueBase> GetSlotSaveFields(int slotId)
        {
            return SlotSaveObjects(slotId)
                .SelectMany(t => t.GetSaveValues())
                .Select(t => t.Value);
        }


        /// <summary>
        /// Initializes a new save slot into the system that wasn't there from the start.
        /// </summary>
        /// <param name="newSlot">The new slot to initialize.</param>
        public static void InitializeNewSaveSlot(SaveSlot newSlot)
        {
            if (!AreSlotsInitialized)
            {
                InitializeSlotObjects(Array.Empty<SaveSlot>());
            }
            
            AllSlotSaveObjects.Add(newSlot, new SlotSaveObjectsContainer());
            CacheSlotSaveValues();
        }


        /// <summary>
        /// Removes a save slot from the setup where it was there before.
        /// </summary>
        /// <param name="slotId">The slot if to remove.</param>
        public static void RemoveSlotObjects(int slotId)
        {
            if (AllSlotSaveObjects.All(t => t.Key.SlotId != slotId)) return;

            var target = AllSlotSaveObjects.First(t => t.Key.SlotId == slotId);
            AllSlotSaveValues.Remove(target.Key);
            AllSlotSaveObjects.Remove(target.Key);
        }
    }
}