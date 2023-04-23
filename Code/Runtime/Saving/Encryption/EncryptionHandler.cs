﻿/*
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

using System.IO;
using System.Security.Cryptography;

namespace CarterGames.Assets.SaveManager.Encryption
{
    /// <summary>
    /// Handles the encryption & Decryption of the save data when in use. 
    /// </summary>
    public static class EncryptionHandler
    {
        /// <summary>
        /// Encrypts the data when called.
        /// </summary>
        /// <param name="jsonData">The json string to use.</param>
        public static void EncryptData(string jsonData)
        {
            switch (AssetAccessor.GetAsset<SettingsAssetRuntime>().Encryption)
            {
                case EncryptionOption.Aes:

                    var iAes = Aes.Create();
                    
                    using (var fStream = new FileStream(AssetAccessor.GetAsset<SettingsAssetRuntime>().SavePath, FileMode.OpenOrCreate))
                    {
                        if (!AssetAccessor.GetAsset<EncryptionKeyAsset>().HasKey)
                        {
                            AssetAccessor.GetAsset<EncryptionKeyAsset>().SaveEncryptionKey = iAes.Key;
                        }
                        else
                        {
                            iAes.KeySize = 256;
                            iAes.Key = AssetAccessor.GetAsset<EncryptionKeyAsset>().SaveEncryptionKey;
                        }
                        
                        
                        var inputIv = iAes.IV;
                        fStream.Write(inputIv, 0, inputIv.Length);
                        
                        var iStream = new CryptoStream(fStream, iAes.CreateEncryptor(iAes.Key, iAes.IV), CryptoStreamMode.Write);
                        
                        using (var writer = new StreamWriter(iStream))
                        {
                            writer.WriteLine(jsonData);
                        }
                        
                        iStream.Close();
                    }

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

                    var fStream = new FileStream(AssetAccessor.GetAsset<SettingsAssetRuntime>().SavePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    var oAes = Aes.Create();
                    var outputIv = new byte[oAes.IV.Length];


                    // Read the IV from the file.
                    fStream.Read(outputIv, 0, outputIv.Length);

                    // Create CryptoStream, wrapping FileStream
                    var oStream = new CryptoStream(fStream, oAes.CreateDecryptor(AssetAccessor.GetAsset<EncryptionKeyAsset>().SaveEncryptionKey, outputIv), CryptoStreamMode.Read);
            
                    var reader = new StreamReader(fStream);
                    var text = reader.ReadToEnd();
            
                    reader.Close();
                    oStream.Close();
                    fStream.Close();

                    return text;
                
                case EncryptionOption.Disabled:
                default:
                    return string.Empty;
            }
        }
    }
}