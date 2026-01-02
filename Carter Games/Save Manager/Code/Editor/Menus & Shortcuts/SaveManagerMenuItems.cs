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

using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class SaveManagerMenuItems
    {
        [MenuItem("Tools/Carter Games/Save Manager/Load Save Data", priority = 50)]
        public static void ManualLoadGame()
        {
            SaveManager.LoadGame();
        }
        
        
        [MenuItem("Tools/Carter Games/Save Manager/Save, Save Data", priority = 51)]
        public static void ManualSaveGame()
        {
            SaveManager.SaveGame();
        }


        // [MenuItem("Tools/Carter Games/Save Manager/Reset All Save Data", priority = 33)]
        // public static void ManualResetData()
        // {
        //     OldSaveManager.Clear();
        // }
    }
}