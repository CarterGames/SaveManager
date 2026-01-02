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
using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Handles save objects in editor space specifically.
    /// </summary>
    public static class EditorSaveObjectController
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static Dictionary<SaveObject, SaveObjectEditor> globalEditorsLookup;
        private static Dictionary<int, Dictionary<SaveObject, SaveObjectEditor>> slotEditorsLookup;
        private static Dictionary<SaveObject, List<string>> saveValueKeys;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets if the editor setup has already initialized.
        /// </summary>
        private static bool IsEditorInitialized { get; set; }
        
                
        /// <summary>
        /// Gets if the save object controller & the editor setup has initialized.
        /// </summary>
        public static bool IsInitialized => SaveObjectController.IsInitialized && IsEditorInitialized;

        
        /// <summary>
        /// Gets if global save objects exist.
        /// </summary>
        public static bool HasGlobalSaveObjects => SaveObjectController.HasGlobalSaveObjects;
        
        
        /// <summary>
        /// Gets if slot save objects exist.
        /// </summary>
        public static bool HasSlotSaveObjects => SaveObjectController.HasSlotSaveObjects;
        
        
        /// <summary>
        /// Gets the global save object.
        /// </summary>
        public static IEnumerable<SaveObject> GlobalSaveObjects => SaveObjectController.GlobalSaveObjects;
        
        
        /// <summary>
        /// Gets the slot save objects (for all slots)
        /// </summary>
        public static IEnumerable<SaveObject> SlotSaveObjects => SaveObjectController.AllSlotSaveObjects
            .SelectMany(t => t.Value.SlotSaveObjects);
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raised when the editor setup for save objects is initialized.
        /// </summary>
        public static readonly Evt InitializedEditorEvt = new Evt();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Runs the initialization on editor load.
        /// </summary>
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            if (SaveManagerInitializer.IsInitialized) return;
            
            SaveManagerInitializer.InitializedEvt.Add(OnRuntimeElementsInitialized);

            try
            {
                SaveManagerInitializer.InitializeSaveManagerRuntimeSetup();
            }
            catch (Exception e)
            {
                SmDebugLogger.LogDev($"An issue occurred on save object init.\nMsg: {e.Message}\nTrace: {e.StackTrace}");
            }
            
            return;

            void OnRuntimeElementsInitialized()
            {
                SaveManagerInitializer.InitializedEvt.Remove(OnRuntimeElementsInitialized);
                InitializeEditor();
                
                IsEditorInitialized = true;
                InitializedEditorEvt.Raise();
            }
        }
        
        
        /// <summary>
        /// Runs the editor side initialization.
        /// </summary>
        private static void InitializeEditor()
        {
            globalEditorsLookup = new Dictionary<SaveObject, SaveObjectEditor>();
            slotEditorsLookup = new Dictionary<int, Dictionary<SaveObject, SaveObjectEditor>>();

            foreach (var entry in SaveObjectController.GlobalSaveObjects)
            {
                globalEditorsLookup.Add(entry, (SaveObjectEditor)UnityEditor.Editor.CreateEditor(entry));
            }
            
            if (SaveSlotManager.HasAnySlots)
            {
                foreach (var entry in SaveObjectController.AllSlotSaveObjects)
                {
                    foreach (var saveObject in entry.Value.SlotSaveObjects)
                    {
                        if (!slotEditorsLookup.ContainsKey(entry.Key.SlotId))
                        {
                            slotEditorsLookup.Add(entry.Key.SlotId, new Dictionary<SaveObject, SaveObjectEditor>());
                        }

                        slotEditorsLookup[entry.Key.SlotId].Add(saveObject,
                            (SaveObjectEditor)UnityEditor.Editor.CreateEditor(saveObject));
                    }
                }
            }

            GetAllSaveValueKeys();
        }
        

        /// <summary>
        /// Gets the save objects for a particular slot.
        /// </summary>
        /// <param name="id">The slot id to get.</param>
        /// <returns>IEnumerable of SaveObject</returns>
        public static IEnumerable<SaveObject> GetSlotSaveObjects(int id)
        {
            return SaveObjectController.SlotSaveObjects(id);
        }


        /// <summary>
        /// Tries to get the editor class for a save object.
        /// </summary>
        /// <param name="saveObject">The save object to get for.</param>
        /// <param name="editor">The editor class for that save object.</param>
        /// <returns>Bool</returns>
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
        
        
        /// <summary>
        /// Tries to get the editor class for a save object of a slot.
        /// </summary>
        /// <param name="slotId">The slot id to get.</param>
        /// <param name="saveObjectType">The save object type to get.</param>
        /// <param name="editor">The editor class for that save object.</param>
        /// <returns>Bool</returns>
        public static bool TryGetEditorForSlotObjectType(int slotId, Type saveObjectType, out SaveObjectEditor editor)
        {
            editor = null;

            if (!IsInitialized) return false;

            if (slotEditorsLookup.ContainsKey(slotId))
            {
                editor = slotEditorsLookup[slotId].FirstOrDefault(t => t.Key.GetType() == saveObjectType).Value;
                return editor != null;
            }

            return false;
        }
        
        
        /// <summary>
        /// Tries to get the editor class for a save object of a slot.
        /// </summary>
        /// <param name="slotId">The slot id to get.</param>
        /// <param name="saveObject">The save object to get for.</param>
        /// <param name="editor">The editor class for that save object.</param>
        /// <returns>Bool</returns>
        public static bool TryGetEditorForSlotObject(int slotId, SaveObject saveObject, out SaveObjectEditor editor)
        {
            editor = null;

            if (!IsInitialized) return false;

            if (slotEditorsLookup.ContainsKey(slotId))
            {
                editor = slotEditorsLookup[slotId][saveObject];
                return true;
            }

            return false;
        }

        
        /// <summary>
        /// Gets if there are any duplicate keys for any save values to flag to the user.
        /// </summary>
        /// <param name="saveObject">The save object to check for duplicates.</param>
        /// <returns>Bool</returns>
        public static bool HasDuplicateSaveValueKeys(SaveObject saveObject)
        {
            if (!IsEditorInitialized) return false;
            if (!saveValueKeys.ContainsKey(saveObject)) return false;
            return saveValueKeys[saveObject].Distinct().Count() != saveValueKeys[saveObject].Count;
        }


        /// <summary>
        /// Gets all the save value keys for duplicate checks mainly.
        /// </summary>
        /// <returns>Dictionary of SaveObject key List of strings value.</returns>
        private static Dictionary<SaveObject, List<string>> GetAllSaveValueKeys()
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

            return saveValueKeys;
        }

        
        /// <summary>
        /// Adds the editors for a newly created save slot.
        /// </summary>
        /// <param name="slot">The slot to create editors for.</param>
        public static void AddEditorsForSaveSlot(SaveSlot slot)
        {
            var dic = new Dictionary<SaveObject, SaveObjectEditor>();
            
            foreach (var entry in SaveObjectController.AllSlotSaveObjects)
            {
                // Skip slots that are not the inputted slot.
                if (entry.Key != slot) continue;
                
                foreach (var saveObject in entry.Value.SlotSaveObjects)
                {
                    if (!slotEditorsLookup.ContainsKey(entry.Key.SlotId))
                    {
                        slotEditorsLookup.Add(entry.Key.SlotId, new Dictionary<SaveObject, SaveObjectEditor>());
                    }

                    slotEditorsLookup[entry.Key.SlotId].Add(saveObject,
                        (SaveObjectEditor)UnityEditor.Editor.CreateEditor(saveObject));
                }

                return;
            }
        }
        

        /// <summary>
        /// Resets all save objects when called.
        /// </summary>
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