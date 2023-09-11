namespace CarterGames.Assets.SaveManager.Encryption
{
    public interface IEncryptionHandler
    {
        public void Encrypt(string jsonData);
        public string Decrypt();
    }
}