using System;
using System.Collections.Generic;
using System.Linq;
using CarterGames.Assets.SaveManager.Slots;
using CarterGames.Shared.SaveManager;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class EditorSaveObjectController
    {
        private static Dictionary<SaveObject, SaveObjectEditor> globalEditorsLookup;
        private static Dictionary<int, Dictionary<SaveObject, SaveObjectEditor>> slotEditorsLookup;
        private static Dictionary<SaveObject, List<string>> saveValueKeys;

        
        private static bool IsEditorInitialized { get; set; }
        
        
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            if (SaveManagerInitializer.IsInitialized) return;
            
            SaveManagerInitializer.InitializedEvt.Add(OnRuntimeElementsInitialized);
            
            SaveManagerInitializer.InitializeSaveManagerRuntimeSetup();
            
            return;

            void OnRuntimeElementsInitialized()
            {
                SaveManagerInitializer.InitializedEvt.Remove(OnRuntimeElementsInitialized);
                InitializeEditor();
                
                IsEditorInitialized = true;
                InitializedEditorEvt.Raise();
            }
        }

        
        public static bool IsInitialized => SaveObjectController.IsInitialized && IsEditorInitialized;

        public static bool HasGlobalSaveObjects => SaveObjectController.HasGlobalSaveObjects;
        public static bool HasSlotSaveObjects => SaveObjectController.HasSlotSaveObjects;
        public static IEnumerable<SaveObject> GlobalSaveObjects => SaveObjectController.GlobalSaveObjects;
        public static IEnumerable<SaveObject> SlotSaveObjects => SaveObjectController.AllSlotSaveObjects.SelectMany(t => t.Value.SlotSaveObjects);
        public static IEnumerable<SaveObject> GetSlotSaveObjects(int index) => SaveObjectController.SlotSaveObjects(index);
        

        
        public static Evt InitializedEditorEvt = new Evt();

        
        
        private static void InitializeEditor()
        {
            globalEditorsLookup = new Dictionary<SaveObject, SaveObjectEditor>();
            slotEditorsLookup = new Dictionary<int, Dictionary<SaveObject, SaveObjectEditor>>();

            foreach (var entry in SaveObjectController.AllGlobalSaveObjects)
            {
                globalEditorsLookup.Add(entry, (SaveObjectEditor)UnityEditor.Editor.CreateEditor(entry));
            }
            
            if (SaveSlotManager.HasAnySlots)
            {
                foreach (var entry in SaveObjectController.AllSlotSaveObjects)
                {
                    foreach (var saveObject in entry.Value.SlotSaveObjects)
                    {
                        if (!slotEditorsLookup.ContainsKey(entry.Key.SlotIndex))
                        {
                            slotEditorsLookup.Add(entry.Key.SlotIndex, new Dictionary<SaveObject, SaveObjectEditor>());
                        }

                        slotEditorsLookup[entry.Key.SlotIndex].Add(saveObject,
                            (SaveObjectEditor)UnityEditor.Editor.CreateEditor(saveObject));
                    }
                }
            }

            GetAllSaveValueKeys();
        }


        public static bool TryGetEditorForObject(SaveObject saveObject, out SaveObjectEditor editor)
        {
            editor = null;

            if (!IsInitialized) return false;

            if (globalEditorsLookup.ContainsKey(saveObject))
            {
                editor = globalEditorsLookup[saveObject];
                return true;
            }

            return false;
        }
        
        
        public static bool TryGetEditorForSlotObjectType(int slotIndex, Type saveObjectType, out SaveObjectEditor editor)
        {
            editor = null;

            if (!IsInitialized) return false;

            if (slotEditorsLookup.ContainsKey(slotIndex))
            {
                editor = slotEditorsLookup[slotIndex].FirstOrDefault(t => t.Key.GetType() == saveObjectType).Value;
                return editor != null;
            }

            return false;
        }
        
        
        public static bool TryGetEditorForSlotObject(int slotIndex, SaveObject saveObject, out SaveObjectEditor editor)
        {
            editor = null;

            if (!IsInitialized) return false;

            if (slotEditorsLookup.ContainsKey(slotIndex))
            {
                editor = slotEditorsLookup[slotIndex][saveObject];
                return true;
            }

            return false;
        }

        
        public static bool HasDuplicateSaveValueKeys(SaveObject saveObject)
        {
            if (!IsEditorInitialized) return false;
            if (!saveValueKeys.ContainsKey(saveObject)) return false;
            return saveValueKeys[saveObject].Distinct().Count() != saveValueKeys[saveObject].Count;
        }


        private static void GetAllSaveValueKeys()
        {
            saveValueKeys = new Dictionary<SaveObject, List<string>>();

            if (HasGlobalSaveObjects)
            {
                foreach (var entry in GlobalSaveObjects)
                {
                    foreach (var value in entry.GetSaveValues())
                    {
                        if (string.IsNullOrEmpty(value.Value.key)) continue;
                        
                        if (saveValueKeys.ContainsKey(entry))
                        {
                            saveValueKeys[entry].Add(value.Value.key);
                        }
                        else
                        {
                            saveValueKeys.Add(entry, new List<string>()
                            {
                                value.Value.key
                            });
                        }
                    }
                }
            }

            if (HasSlotSaveObjects)
            {
                foreach (var entry in SlotSaveObjects)
                {
                    foreach (var value in entry.GetSaveValues())
                    {
                        if (string.IsNullOrEmpty(value.Value.key)) continue;
                        
                        if (saveValueKeys.ContainsKey(entry))
                        {
                            saveValueKeys[entry].Add(value.Value.key);
                        }
                        else
                        {
                            saveValueKeys.Add(entry, new List<string>()
                            {
                                value.Value.key
                            });
                        }
                    }
                }
            }
        }

        
        public static void AddEditorsForSaveSlot(SaveSlot slot)
        {
            var dic = new Dictionary<SaveObject, SaveObjectEditor>();
            
            foreach (var entry in SaveObjectController.AllSlotSaveObjects)
            {
                // Skip slots that are not the inputted slot.
                if (entry.Key != slot) continue;
                
                foreach (var saveObject in entry.Value.SlotSaveObjects)
                {
                    if (!slotEditorsLookup.ContainsKey(entry.Key.SlotIndex))
                    {
                        slotEditorsLookup.Add(entry.Key.SlotIndex, new Dictionary<SaveObject, SaveObjectEditor>());
                    }

                    slotEditorsLookup[entry.Key.SlotIndex].Add(saveObject,
                        (SaveObjectEditor)UnityEditor.Editor.CreateEditor(saveObject));
                }

                return;
            }
        }
        

        public static void ResetAllObjects()
        {
            foreach (var entry in SaveObjectController.GlobalSaveObjects)
            {
                entry.ResetObjectSaveValues();
            }

            foreach (var slotData in SaveObjectController.AllSlotSaveObjects)
            {
                foreach (var entry in slotData.Value.SlotSaveObjects)
                {
                    entry.ResetObjectSaveValues();
                }
            }
            
            SaveManager.SaveGame();
        }
    }
}