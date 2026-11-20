using Crossdyne.Toolkit.Primitives;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EnigmaVault.Desktop.Services.Secure
{
    internal sealed class KeyManager : IKeyManager
    {
        private readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EnigmaVault", "dek.data");

        private static readonly byte[] s_entropy = Encoding.Unicode.GetBytes("L2MOoWYp1}!iiULj+#]|`YG>+~s3-%n~");

        public void SaveKey(byte[] key)
        {
            string jsonString = JsonSerializer.Serialize(key);
            byte[] plainBytes = Encoding.UTF8.GetBytes(jsonString);

            byte[] encryptedBytes = ProtectedData.Protect(plainBytes, s_entropy, DataProtectionScope.CurrentUser);
            
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            File.WriteAllBytes(_filePath, encryptedBytes);
        }

        public Maybe<byte[]> GetKey()
        {
            if (!File.Exists(_filePath))
                return null;

            try
            {
                byte[] encryptedBytes = File.ReadAllBytes(_filePath);

                byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, s_entropy, DataProtectionScope.CurrentUser);

                string jsonString = Encoding.UTF8.GetString(decryptedBytes);
                var key = JsonSerializer.Deserialize<byte[]>(jsonString);

                if (key == null || key.Length == 0)
                    return null;

                return key;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void ClearKey()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
    }
}