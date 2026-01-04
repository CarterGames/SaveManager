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

using CarterGames.Assets.SaveManager.Helpers;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles saving game data to a local file.
    /// </summary>
    public sealed class SaveLocationLocalFile : ISaveDataLocation
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string EditorSavePath = "%Application.persistentDataPath%/EditorSave/save.sf2";
        private const string SavePath = "%Application.persistentDataPath%/save.sf2";
        private const string SavePathWeb = "/idbfs/%productName%-%companyName%/save.sf2";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the active save path for the local file.
        /// </summary>
        private static string ActiveSavePath
        {
            get
            {
#if UNITY_EDITOR
                return LocalFileHelper.ParseSavePath(EditorSavePath);
#elif UNITY_WEBGL
                return LocalFileHelper.ParseSavePath(SavePathWeb);
#else
                return LocalFileHelper.ParseSavePath(SavePath);
#endif
            }
        }


        /// <summary>
        /// The data location to use for the game save data.
        /// </summary>
        public IDataLocation DataLocation => new DataLocationLocalFile();
        
        
        /// <summary>
        /// Gets if the location has a save data already.
        /// </summary>
        public bool HasSaveData => DataLocation.HasData(ActiveSavePath);
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Saves the data to the location defined in <see cref="DataLocation"/>
        /// </summary>
        /// <param name="json">The json to save to the location.</param>
        public void SaveDataToLocation(string json)
        {
            DataLocation.SaveToLocation(ActiveSavePath, json);
        }
        
        
        /// <summary>
        /// Loads the data from the location defined in <see cref="DataLocation"/>
        /// </summary>
        /// <returns>The data loaded from the location.</returns>
        public string LoadDataFromLocation()
        {
            return DataLocation.LoadFromLocation(ActiveSavePath);
        }
    }
}