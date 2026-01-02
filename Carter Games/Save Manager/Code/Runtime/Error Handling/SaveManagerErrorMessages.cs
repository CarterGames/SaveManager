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

using System.Collections.Generic;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles generating the error messages for the asset.
    /// </summary>
    public static class SaveManagerErrorMessages
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string ErrorCodeFormat = "{0} ({1})";
        private const string ErrorCodeFormatWithMsg = "{0} ({1}): {2}";
        
        private static readonly IReadOnlyDictionary<SaveManagerErrorCode, string> ErrorMessagesLookup =
            new Dictionary<SaveManagerErrorCode, string>()
            {
                { SaveManagerErrorCode.NoGlobalSaveObjectOfType, "No global save object of type {0} found." },
                { SaveManagerErrorCode.NoSlotSaveObjectOfType, "No save slot object of type {0} found." },
                { SaveManagerErrorCode.NoSaveValueFound, "No save value found under the key {0} of type {1}." },
                { SaveManagerErrorCode.NoSaveKeyAssigned, "No save key assigned to the a save value {0}, This value cannot be saved until a unique key is assigned." },
                { SaveManagerErrorCode.DuplicateSaveKeys, "Duplicate save keys detected, please ensure all save keys are unique." },
                { SaveManagerErrorCode.NoSlotsToLoad, "There are no slots defined in the save to load. Skipping save slot loading..." },
                { SaveManagerErrorCode.SaveEncryptionFailed, "The save encryption encountered an error. See more info in trace.\nHandler: {0}\nException: {1}\nTrace: {2}" },
                { SaveManagerErrorCode.SaveDecryptionFailed, "The save decryption encountered an error. See more info in trace.\nHandler: {0}\nException: {1}\nTrace: {2}" },
                { SaveManagerErrorCode.SaveValueLoadFailed, "The save value {0} of type {1} failed to load from the save.\nException: {2}\nTrace: {3}" },
                { SaveManagerErrorCode.SaveValueResetFailed, "The save value {0} of type {1} failed to reset.\nException: {2}\nTrace: {3}" },
                { SaveManagerErrorCode.SaveValueTypeMismatch, "The save value {0} failed to load as the type was not a match. Saved type {1}, expected {2}." },
                { SaveManagerErrorCode.LegacyLoadFailed, "Failed to load a legacy save file. See more info in trace.\nException: {0}\nTrace: {1}" },
                { SaveManagerErrorCode.SaveCaptureSaveFailed, "Failed to save a capture. See more info in trace.\nCapture: {0}\nException: {1}\nTrace: {2}" },
            };

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the error message for the entered error code.
        /// </summary>
        /// <param name="errorCode">The code to get the message for.</param>
        /// <param name="parameters">Any parameters to add into the message.</param>
        /// <returns>The error message.</returns>
        public static string GetErrorMessageFormat(this SaveManagerErrorCode errorCode, params object[] parameters)
        {
            if (ErrorMessagesLookup.TryGetValue(errorCode, out var message))
            {
                var formattedMsg = string.Format(message, parameters);
                return string.Format(ErrorCodeFormatWithMsg, errorCode, (int) errorCode, formattedMsg);
            }

            return string.Format(ErrorCodeFormat, errorCode, (int) errorCode);
        }
    }
}