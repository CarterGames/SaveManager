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

namespace CarterGames.Shared.SaveManager.Editor
{
    public static class SharedPerUserSettings
    {
        private static readonly string AutoValidationAutoCheckId = $"{UniqueId}_CarterGames_AudioManager_Editor_AutoVersionCheck";
        
        

        /// <summary>
        /// The unique if for the assets settings to be per project...
        /// </summary>
        public static string UniqueId =>
            (string) PerUserSettingsEditor.GetOrCreateValue<string>(PerUserSettingsUuidHandler.Uuid,
                PerUserSettingType.PlayerPref, Guid.NewGuid().ToString());


        /// <summary>
        /// Has the scanner been initialized.
        /// </summary>
        public static bool VersionValidationAutoCheckOnLoad
        {
            get => (bool) PerUserSettingsEditor.GetOrCreateValue<bool>(AutoValidationAutoCheckId, PerUserSettingType.EditorPref);
            set => PerUserSettingsEditor.SetValue<bool>(AutoValidationAutoCheckId, PerUserSettingType.EditorPref, value);
        }
    }
}