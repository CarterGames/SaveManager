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

using System;
using CarterGames.Assets.SaveManager.Encryption;
using CarterGames.Shared.SaveManager;

namespace CarterGames.Assets.SaveManager
{
    // An extension for the Encryption API for the Save Manager class.
    public static partial class SaveManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets if the save should be encrypted.
        /// </summary>
        private static bool EncryptSave => SmAssetAccessor.GetAsset<DataAssetSettings>().EncryptSave;
        
        
        /// <summary>
        /// Gets if the encryption handler has been assigned.
        /// </summary>
        private static bool HasHandler => SmAssetAccessor.GetAsset<DataAssetSettings>().HasEncryptionHandler;
        
        
        /// <summary>
        /// Gets the assigned encryption handler.
        /// </summary>
        private static ISaveEncryptionHandler EncryptionHandler => 
            SmAssetAccessor.GetAsset<DataAssetSettings>().EncryptionHandler;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Tries to encrypt the save content with the active handler.
        /// </summary>
        /// <param name="content">The content to encrypt.</param>
        /// <param name="encryptedContent">The content encrypted.</param>
        /// <returns>If it was successful or not.</returns>
        private static bool TryEncryptSaveContent(string content, out string encryptedContent)
        {
            encryptedContent = content;

            if (!EncryptSave) return false;
            if (!HasHandler) return false;

            try
            {
                encryptedContent = EncryptionHandler.Encrypt(content);
                return true;
            }
            catch (Exception e)
            {
                SmDebugLogger.LogError(SaveManagerErrorCode.SaveEncryptionFailed.GetErrorMessageFormat(EncryptionHandler.ToString(), e.Message, e.StackTrace));
                return false;
            }
        }
        
        
        /// <summary>
        /// Tries to decrypt the save content with the active handler.
        /// </summary>
        /// <param name="content">The content to decrypt.</param>
        /// <param name="decryptedContent">The content decrypted.</param>
        /// <returns>If it was successful or not.</returns>
        private static bool TryDecryptSaveContent(string content, out string decryptedContent)
        {
            decryptedContent = content;
            
            if (!EncryptSave) return false;
            if (!HasHandler) return false;

            try
            {
                decryptedContent = EncryptionHandler.Decrypt(content);
                return true;
            }
            catch (Exception e)
            {
                SmDebugLogger.LogError(SaveManagerErrorCode.SaveDecryptionFailed.GetErrorMessageFormat(EncryptionHandler.ToString(), e.Message, e.StackTrace));
                return false;
            }
        }
    }
}