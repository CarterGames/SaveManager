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

using UnityEngine;

namespace CarterGames.Assets.SaveManager.Demo
{
    [SaveCategory("Example (Safe to delete)")]
    public class ExampleSaveObject : SaveObject
    {
        public SaveValue<string> playerName = new SaveValue<string>("examplePlayerName");
        public SaveValue<int> playerHealth = new SaveValue<int>("examplePlayerHealth");
        public SaveValue<Vector3> playerPosition = new SaveValue<Vector3>("examplePlayerPosition");
        public SaveValue<int> playerShield = new SaveValue<int>("examplePlayerShield");
    }
}