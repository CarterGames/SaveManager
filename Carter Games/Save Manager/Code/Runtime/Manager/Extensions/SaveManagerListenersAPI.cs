/*
 * Save Manager
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

using CarterGames.Shared.SaveManager;

namespace CarterGames.Assets.SaveManager
{
    // An extension for any listeners for the save system.
    public static partial class SaveManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Sets up the listeners for use.
        /// </summary>
        private static void InitializeListeners()
        {
            foreach (var entry in AssemblyHelper.GetClassesOfType<IGameSaveListener>(false))
            {
                GameSaveStartedEvt.Add(entry.OnGameSaveCalled);
                GameSaveCompletedEvt.Add(entry.OnGameSaveCompleted);
            }
            
            foreach (var entry in AssemblyHelper.GetClassesOfType<IGameLoadListener>(false))
            {
                GameLoadStartedEvt.Add(entry.OnGameLoadCalled);
                GameLoadFailedEvt.Add(entry.OnGameLoadFailed);
                GameLoadFailedCompletelyEvt.Add(entry.OnGameLoadFailedCompletely);
                GameLoadedEvt.Add(entry.OnGameLoadCompleted);
            }
        }
    }
}