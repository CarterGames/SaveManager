/*
 * Copyright (c) 2024 Carter Games
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
    public class AesEncryption : IEncryptionHandler
    {
        /// <summary>
        /// Encrypts the data.
        /// </summary>
        /// <param name="jsonData">The data to set.</param>
        public void Encrypt(string jsonData)
        {
            var iAes = Aes.Create();
                    
            using (var fStream = new FileStream(AssetAccessor.GetAsset<AssetGlobalRuntimeSettings>().SavePath, FileMode.OpenOrCreate))
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
                
                iAes.GenerateIV();
                
                var inputIv = iAes.IV;
                fStream.Write(inputIv, 0, inputIv.Length);
                        
                var iStream = new CryptoStream(fStream, iAes.CreateEncryptor(iAes.Key, iAes.IV), CryptoStreamMode.Write);
                
                using (var writer = new StreamWriter(iStream))
                {
                    writer.WriteLine(jsonData);
                }
                        
                iStream.Close();
            }
        }

        
        /// <summary>
        /// Decrypts the data.
        /// </summary>
        /// <returns>The decrypted data.</returns>
        public string Decrypt()
        {
            var fStream = new FileStream(AssetAccessor.GetAsset<AssetGlobalRuntimeSettings>().SavePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var oAes = Aes.Create();

            oAes.Key = AssetAccessor.GetAsset<EncryptionKeyAsset>().SaveEncryptionKey;
            
            var outputIv = new byte[oAes.IV.Length];
            
            // Read the IV from the file.
            fStream.Read(outputIv, 0, outputIv.Length);
            oAes.IV = outputIv;

            // Create CryptoStream, wrapping FileStream
            var oStream = new CryptoStream(fStream, oAes.CreateDecryptor(oAes.Key, oAes.IV), CryptoStreamMode.Read);
            
            using (var reader = new StreamReader(oStream))
            {
                var text = reader.ReadToEnd();
                
                oStream.Close();
                fStream.Close();

                return text;
            }
        }
    }
}