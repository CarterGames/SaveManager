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

using CarterGames.Assets.SaveManager.Slots;
using UnityEngine;
using UnityEngine.UI;

namespace CarterGames.Assets.SaveManager.Demo
{
    public class ExampleSlotDisplay : MonoBehaviour
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private GameObject newSlotContainer;
        [SerializeField] private GameObject activeSlotContainer;

        [SerializeField] private Text slotTitleLabel;
        [SerializeField] private Text slotPlaytimeLabel;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private int SlotIndex { get; set; }
        private bool HasSlotData => DisplayedSlot != null;
        private SaveSlot DisplayedSlot { get; set; }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Sets the slot id.
        /// </summary>
        /// <param name="id">The id to set to show.</param>
        public void SetSlotId(int id)
        {
            SlotIndex = id;
        }
        
        
        /// <summary>
        /// Assigns the save slot to this display.
        /// </summary>
        /// <param name="slot"></param>
        public void AssignSlot(SaveSlot slot)
        {
            SlotIndex = slot.SlotId;
            DisplayedSlot = slot;
            UpdateDisplay();
        }
        
        
        /// <summary>
        /// Creates a new slot at the slot index when called.
        /// </summary>
        /// <remarks>
        /// Called from button on slot display to create a new slot
        /// </remarks>
        public void CreateNewSlot()
        {
            if (SaveSlotManager.TryCreateSlotAtIndex(SlotIndex, out var slot))
            {
                AssignSlot(slot);
            }
        }


        /// <summary>
        /// Updates the slot display to the current state of the slot index.
        /// </summary>
        public void UpdateDisplay()
        {
            if (!HasSlotData)
            {
                newSlotContainer.SetActive(true);
                activeSlotContainer.SetActive(false);
            }
            else
            {
                newSlotContainer.SetActive(false);
                activeSlotContainer.SetActive(true);
                
                slotTitleLabel.text = string.Format(slotTitleLabel.text, DisplayedSlot.SlotId);
                slotPlaytimeLabel.text = DisplayedSlot.Playtime.ToString();
            }
        }


        /// <summary>
        /// "Loads" the slot when called
        /// </summary>
        /// <remarks>
        /// Is called via GUI button, would then go to load the game scenes etc after being called etc.
        /// </remarks>
        public void LoadSlot()
        {
            if (!HasSlotData) return;
            SaveSlotManager.LoadSlot(DisplayedSlot.SlotId);
            UpdateDisplay();
        }


        /// <summary>
        /// Deletes the slot when called.
        /// </summary>
        /// <remarks>
        /// Is called from a GUI button on the slot display.
        /// </remarks>
        public void DeleteSlot()
        {
            if (!HasSlotData) return;
            SaveSlotManager.DeleteSlot(DisplayedSlot.SlotId);
            DisplayedSlot = null;
            UpdateDisplay();
        }
    }
}