/*
 * Copyright (c) 2018-Present Carter Games
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

using UnityEngine;

namespace CarterGames.Assets.SaveManager.Utility
{
    /// <summary>
    /// Handles any runtime utility functionality.
    /// </summary>
    public static class UtilRuntime
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        // Encryption
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        private const string CurrentEncryptionId = "Carter_Games_Save_Manager_Current_Encryption_Option";
        private const string BuildEncryptionId = "Carter_Games_Save_Manager_LastBuild_Encryption_Option";
        private const string CurrentEditorEncryptionId = "Carter_Games_Save_Manager_EditorCurrent_Encryption_Option";
        private const string EditorEncryptionId = "Carter_Games_Save_Manager_Editor_Encryption_Option";

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if the pref exists.
        /// </summary>
        /// <param name="setting">The setting to get.</param>
        /// <param name="option">The option to use.</param>
        /// <returns>If the pref exists.</returns>
        public static bool HasPref(PrefSetting setting, PrefUseOption option)
        {
            switch (setting)
            {
                case PrefSetting.Encryption:

                    switch (option)
                    {
                        case PrefUseOption.Current:
                            return PlayerPrefs.HasKey(CurrentEncryptionId);
                        case PrefUseOption.Build:
                            return PlayerPrefs.HasKey(BuildEncryptionId);
                        case PrefUseOption.Editor:
                            return PlayerPrefs.HasKey(EditorEncryptionId);
                        case PrefUseOption.EditorCurrent:
                            return PlayerPrefs.HasKey(CurrentEditorEncryptionId);
                        default:
                            return false;
                    }

                default:
                    return false;
            }
        }
        

        /// <summary>
        /// Gets the pref exists setting.
        /// </summary>
        /// <param name="setting">The setting to get.</param>
        /// <param name="option">The option to use.</param>
        /// <returns>The current pref value.</returns>
        public static int GetPref(PrefSetting setting, PrefUseOption option)
        {
            switch (setting)
            {
                case PrefSetting.Encryption:

                    switch (option)
                    {
                        case PrefUseOption.Current:
                            return PlayerPrefs.GetInt(CurrentEncryptionId);
                        case PrefUseOption.Build:
                            return PlayerPrefs.GetInt(BuildEncryptionId);
                        case PrefUseOption.Editor:
                            return PlayerPrefs.GetInt(EditorEncryptionId);
                        case PrefUseOption.EditorCurrent:
                            return PlayerPrefs.GetInt(CurrentEditorEncryptionId);
                        default:
                            return -1;
                    }

                default:
                    return -1;
            }
        }
        
        
        /// <summary>
        /// Sets the pref exists setting.
        /// </summary>
        /// <param name="setting">The setting to get.</param>
        /// <param name="option">The option to use.</param>
        /// <param name="value">The value to set to.</param>
        /// <returns>Sets the pref value.</returns>
        public static void SetPref(PrefSetting setting, PrefUseOption option, int value)
        {
            switch (setting)
            {
                case PrefSetting.Encryption:

                    switch (option)
                    {
                        case PrefUseOption.Current:
                            PlayerPrefs.SetInt(CurrentEncryptionId, value);
                            PlayerPrefs.Save();
                            break;
                        case PrefUseOption.Build:
                            PlayerPrefs.SetInt(BuildEncryptionId, value);
                            PlayerPrefs.Save();
                            break;
                        case PrefUseOption.Editor:
                            PlayerPrefs.SetInt(EditorEncryptionId, value);
                            PlayerPrefs.Save();
                            break;
                        case PrefUseOption.EditorCurrent:
                            PlayerPrefs.SetInt(CurrentEditorEncryptionId, value);
                            PlayerPrefs.Save();
                            break;
                        default:
                            break;
                    }
                    
                    break;
                
                default:
                    break;
            }
        }
    }
}