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
        }

        
        /// <summary>
        /// Decrypts the data.
        /// </summary>
        /// <returns>The decrypted data.</returns>
        public string Decrypt()
        {
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
        }
    }
}