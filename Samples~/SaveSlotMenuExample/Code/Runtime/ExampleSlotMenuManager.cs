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

namespace CarterGames.Assets.SaveManager.Demo
{
    /// <summary>
    /// Handles the main slot menu logs when actions are performed.
    /// </summary>
    public sealed class ExampleSlotMenuManager : MonoBehaviour
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnEnable()
        {
            SaveSlotManager.SlotLoadedEvt.Add(OnSlotLoadedSuccessfully);
            SaveSlotManager.SlotLoadFailedEvt.Add(OnSlotLoadFailed);
        }


        private void OnDisable()
        {
            SaveSlotManager.SlotLoadedEvt.Remove(OnSlotLoadedSuccessfully);
            SaveSlotManager.SlotLoadFailedEvt.Remove(OnSlotLoadFailed);
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnSlotLoadedSuccessfully(int loadedSlotIndex)
        {
            // Here you'd run your logic to load the game from the current slot data.
            // You may want to add a dialog that shows the game loaded successfully as well.
            Debug.Log($"Slot {loadedSlotIndex} loaded, game would now move to load & change scene etc.");
        }


        private void OnSlotLoadFailed(int loadedSlotIndex)
        {
            // Here you'd show a load failed dialog.
            // Unlikely to ever occur, but in place in-case of an issue with the slot you are trying to load not being loaded.
            Debug.Log($"Slot {loadedSlotIndex} failed to load, game should show a failed dialog here.");
        }
    }
}