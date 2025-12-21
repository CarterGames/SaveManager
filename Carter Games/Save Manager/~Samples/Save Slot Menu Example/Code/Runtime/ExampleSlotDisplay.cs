/*
 * Copyright (c) 2025 Carter Games
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

using CarterGames.Assets.SaveManager.Slots;
using UnityEngine;
using UnityEngine.UI;

namespace CarterGames.Assets.SaveManager.Demo
{
    public class ExampleSlotDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject newSlotContainer;
        [SerializeField] private GameObject activeSlotContainer;

        [SerializeField] private Text slotTitleLabel;
        [SerializeField] private Text slotPlaytimeLabel;


        private int SlotIndex => transform.GetSiblingIndex();
        private bool HasSlotData => DisplayedSlot != null;
        private SaveSlot DisplayedSlot { get; set; }


        public void AssignSlot(SaveSlot slot)
        {
            DisplayedSlot = slot;
            UpdateDisplay();
        }
        
        
        // Called from button on slot display to create a new slot
        public void CreateNewSlot()
        {
            if (SaveSlotManager.TryCreateSlotAtIndex(SlotIndex, out var slot))
            {
                AssignSlot(slot);
            }
        }


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
                
                slotTitleLabel.text = string.Format(slotTitleLabel.text, DisplayedSlot.SlotIndex);
                slotPlaytimeLabel.text = DisplayedSlot.Playtime.ToString();
            }
        }


        public void LoadSlot()
        {
            if (!HasSlotData) return;
            SaveSlotManager.LoadSlot(DisplayedSlot.SlotIndex);
        }


        public void DeleteSlot()
        {
            if (!HasSlotData) return;
            SaveSlotManager.DeleteSlot(DisplayedSlot.SlotIndex);
        }
    }
}