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
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
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