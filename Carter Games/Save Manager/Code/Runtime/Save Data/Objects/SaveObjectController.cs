using System;
using System.Collections.Generic;
using System.Linq;
using CarterGames.Assets.SaveManager.Slots;
using CarterGames.Shared.SaveManager;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    public static class SaveObjectController
    {
        public static bool IsInitialized { get; set; }
        public static bool AreSlotsInitialized { get; set; }

        public static bool HasGlobalSaveObjects
        {
            get
            {
                if (!IsInitialized) return false;
                return AllGlobalSaveObjects.Count > 0;
            }
        }
        
        public static List<SaveObject> AllGlobalSaveObjects { get; private set; }
        
        public static Dictionary<SaveSlot, SlotSaveObjectsContainer> AllSlotSaveObjects { get; private set; }
        
        public static Dictionary<SaveObject, List<SaveValueBase>> AllGlobalSaveValues { get; private set; }
        
        public static Dictionary<SaveSlot, Dictionary<SaveObject, List<SaveValueBase>>> AllSlotSaveValues { get; private set; }

        public static IEnumerable<Type> GlobalSaveObjectTypes { get; private set; }
        public static IEnumerable<Type> SlotSaveObjectTypes { get; private set; }
        
        public static IEnumerable<SaveObject> GlobalSaveObjects
        {
            get
            {
                if (!IsInitialized) return new List<SaveObject>();
                return AllGlobalSaveObjects;
            }
        }
        
        public static bool HasSlotSaveObjects
        {
            get
            {
                if (!AreSlotsInitialized) return false;
                return AllSlotSaveObjects.Count > 0;
            }
        }


        public static IEnumerable<SaveObject> SlotSaveObjects(int index)
        {
            if (!AreSlotsInitialized) return new List<SaveObject>();

            return AllSlotSaveObjects
                .Where(t=> t.Key.SlotIndex == index)
                .Select(t => t.Value)
                .SelectMany(x => x.SlotSaveObjects);
        }


        public static readonly Evt InitializedEvt = new Evt();
        public static readonly Evt SlotsInitializedEvt = new Evt();


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
            SlotsInitializedEvt.Raise();//
        }


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
        
        
        public static IEnumerable<SaveValueBase> GetGlobalSaveFields()
        {
            return AllGlobalSaveValues
                .Where(t => GlobalSaveObjects.Contains(t.Key))
                .Where(t => t.Value.Any())
                .SelectMany(t => t.Value);
        }
        
        
        public static IEnumerable<SaveValueBase> GetSlotSaveFields(int slotIndex)
        {
            return SlotSaveObjects(slotIndex).SelectMany(t => t.GetSaveValues()).Select(t => t.Value);
        }


        public static void InitializeNewSaveSlot(SaveSlot newSlot)
        {
            if (!AreSlotsInitialized)
            {
                InitializeSlotObjects(Array.Empty<SaveSlot>());
            }
            
            AllSlotSaveObjects.Add(newSlot, new SlotSaveObjectsContainer());
            CacheSlotSaveValues();
        }


        public static void RemoteSlotObjects(int slotIndexToRemove)
        {
            if (AllSlotSaveObjects.All(t => t.Key.SlotIndex != slotIndexToRemove)) return;

            var target = AllSlotSaveObjects.First(t => t.Key.SlotIndex == slotIndexToRemove);
            AllSlotSaveValues.Remove(target.Key);
            AllSlotSaveObjects.Remove(target.Key);
        }
    }
}