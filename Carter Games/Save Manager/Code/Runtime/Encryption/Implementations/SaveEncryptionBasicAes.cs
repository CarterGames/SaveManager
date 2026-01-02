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
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace CarterGames.Assets.SaveManager.Encryption
{
    /// <summary>
    /// A basic AES encryption setup to encrypt the save content data.
    /// </summary>
    /// <remarks>Works similar to the 2.x version.</remarks>
    public sealed class SaveEncryptionBasicAes : ISaveEncryptionHandler
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string DataPref = "cg_sm_encryption_aes_data";
        private const string KeyPref = "key";
        private const string IvPref = "iv";

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the key to use for the AES encryption.
        /// </summary>
        private static byte[] Key => GetKeyOrIv(KeyPref);
        
        
        /// <summary>
        /// Gets the iv to use for the AES encryption.
        /// </summary>
        private static byte[] Iv => GetKeyOrIv(IvPref);
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Handles getting either the key or iv for the AES encryption setup.
        /// </summary>
        /// <param name="envVar">Either Key or IV.</param>
        /// <returns>The relevant data as byte[]</returns>
        private static byte[] GetKeyOrIv(string envVar)
        {
            var value = Environment.GetEnvironmentVariable(DataPref);

            // Makes the data if it doesn't exist yet.
            // Once made it will be the same.
            if (string.IsNullOrEmpty(value))
            {
                var aes = Aes.Create();
                aes.KeySize = 256;
            
                var keyArray = new JArray();
                foreach (var b in aes.Key)
                {
                    keyArray.Add(b);
                }
            
                var ivArray = new JArray();
                foreach (var b in aes.IV)
                {
                    ivArray.Add(b);
                }
            
                Environment.SetEnvironmentVariable(DataPref, new JObject()
                {
                    ["key"] = keyArray,
                    ["iv"] = ivArray,
                }.ToString());
                
                value = Environment.GetEnvironmentVariable(DataPref);
            }
            
            return JToken.Parse(value)[envVar]?.ToObject<byte[]>();
        }


        /// <summary>
        /// Encrypts the save content when called.
        /// </summary>
        /// <param name="contentData">The content to encrypt.</param>
        /// <returns>The encrypted data</returns>
        public string Encrypt(string contentData)
        {
            if (string.IsNullOrEmpty(contentData))
            {
                return string.Empty;
            }

            SmDebugLogger.LogDev($"Data pre-encryption:\n{contentData}");
            
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = Key;
            aes.IV = Iv;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(contentData);
            }
            
            var encryptedData = Convert.ToBase64String(ms.ToArray());
            SmDebugLogger.LogDev($"Data post-encryption:\n{encryptedData}");
            return encryptedData;
        }

    
        /// <summary>
        /// Decrypts the save content when called.
        /// </summary>
        /// <param name="encryptedData">The encrypted data to decrypt.</param>
        /// <returns>The decrypted data</returns>
        public string Decrypt(string encryptedData)
        {
            SmDebugLogger.LogDev($"Data pre-decryption:\n{encryptedData}");
            
            if (string.IsNullOrEmpty(encryptedData))
            {
                return string.Empty;
            }

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = Key;
            aes.IV = Iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(encryptedData));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            
            var data = sr.ReadToEnd();
            SmDebugLogger.LogDev($"Data post-decryption:\n{data}");
            return data;
        }
    }
}