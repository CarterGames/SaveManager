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

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// A constants class for specific ids for ease of use. Mainly for internal asset use.
    /// </summary>
    public static class SaveManagerConstants
    {
        public const string PrefFormat = "CG_SM_{0}";
        
        
        // Asset Prefs
        public const string LogsPref = "logs";
        public const string DevLogsPref = "dev_logs";
        
        public static readonly string LastSaveLocationPrefKey = "last_save_location_used";
        
            
        // Editor icons
        public const string WarningIcon = "editor_warning_triangle";
        public const string BookIcon = "editor_book";
        public const string CogIcon = "editor_cog";
        public const string DataIcon = "editor_data";
        public const string WindowIcon = "editor_window_icon";

        public const string NoCategoryTag = "Uncategorized";
        public const string SettingsWindowPath = "Carter Games/Save Manager";
    }
}