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

using System;
using System.Collections.Generic;

namespace CarterGames.Assets.SaveManager.Encryption
{
    /// <summary>
    /// Handles the encryption & Decryption of the save data when in use. 
    /// </summary>
    public static class EncryptionHandler
    {
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Fields
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        private static Dictionary<EncryptionOption, IEncryptionHandler> handlers;

        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Methods
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */

        /// <summary>
        /// Gets the handler for the current encryption setup.
        /// </summary>
        /// <param name="encryptionOption">The option to use.</param>
        /// <param name="type">The type to get.</param>
        /// <returns>The handler gotten.</returns>
        private static IEncryptionHandler GetHandler(EncryptionOption encryptionOption, Type type)
        {
            handlers ??= new Dictionary<EncryptionOption, IEncryptionHandler>();

            if (handlers.ContainsKey(encryptionOption))
            {
                return handlers[encryptionOption];
            }
            
            handlers.Add(encryptionOption, (IEncryptionHandler) Activator.CreateInstance(type, new object[]{}));
            return handlers[encryptionOption];
        }
        


        /// <summary>
        /// Encrypts the data when called.
        /// </summary>
        /// <param name="jsonData">The json string to use.</param>
        public static void EncryptData(string jsonData)
        {
            switch (AssetAccessor.GetAsset<SettingsAssetRuntime>().Encryption)
            {
                case EncryptionOption.Aes:
                    GetHandler(EncryptionOption.Aes, typeof(AesEncryption)).Encrypt(jsonData);
                    break;
                case EncryptionOption.Disabled:
                default:
                    return;
            }
        }


        /// <summary>
        /// Decrypts the data when called.
        /// </summary>
        /// <returns>The decrypted data.</returns>
        public static string Decrypt(EncryptionOption option)
        {
            switch (option)
            {
                case EncryptionOption.Aes:
                    return GetHandler(EncryptionOption.Aes, typeof(AesEncryption)).Decrypt();
                case EncryptionOption.Disabled:
                default:
                    return string.Empty;
            }
        }
    }
}