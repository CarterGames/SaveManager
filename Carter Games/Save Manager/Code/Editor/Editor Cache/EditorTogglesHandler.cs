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

using CarterGames.Shared.SaveManager.Editor;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class EditorTogglesHandler
    {
        public static bool IsSlotExpanded(string key, int slotId)
        {
            return (bool) PerUserSettingsEditor.GetOrCreateValue<bool>(string.Format(key, slotId), PerUserSettingType.EditorPref);
        }


        public static void SetSlotExpanded(string key, int slotId, bool value)
        {
            PerUserSettingsEditor.SetValue<bool>(string.Format(key, slotId), PerUserSettingType.EditorPref, value);
        }
        
        
        public static bool IsSlotSaveDataExpanded(string key, int slotId)
        {
            return (bool) PerUserSettingsEditor.GetOrCreateValue<bool>(string.Format(key, slotId), PerUserSettingType.EditorPref);
        }
        
        
        public static void SetSlotSaveDataExpanded(string key, int slotId, bool value)
        {
            PerUserSettingsEditor.SetValue<bool>(string.Format(key, slotId), PerUserSettingType.EditorPref, value);
        }
        
        
        public static bool GetSaveObjectIsExpanded(SaveObject saveObject)
        {
            return (bool)PerUserSettingsEditor.GetOrCreateValue<bool>($"cg_sm_so_{saveObject.GetType().Name}_is_expanded",
                PerUserSettingType.EditorPref);
        }
        
        
        public static bool GetSaveObjectIsExpanded(SaveObject saveObject, int slotIndex)
        {
            return (bool)PerUserSettingsEditor.GetOrCreateValue<bool>($"cg_sm_slot_{slotIndex}_so_{saveObject.GetType().Name}_is_expanded",
                PerUserSettingType.EditorPref);
        }
        
        
        public static void SetSaveObjectIsExpanded(SaveObject saveObject, bool value)
        {
            PerUserSettingsEditor.SetValue<bool>($"cg_sm_so_{saveObject.GetType().Name}_is_expanded",
                PerUserSettingType.EditorPref, value);
        }
        
        
        public static void SetSaveObjectIsExpanded(SaveObject saveObject, int slotIndex, bool value)
        {
            PerUserSettingsEditor.SetValue<bool>($"cg_sm_slot_{slotIndex}_so_{saveObject.GetType().Name}_is_expanded",
                PerUserSettingType.EditorPref, value);
        }
    }
}