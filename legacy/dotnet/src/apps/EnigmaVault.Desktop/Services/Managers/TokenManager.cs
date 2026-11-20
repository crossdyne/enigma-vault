using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Desktop.Models;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EnigmaVault.Desktop.Services.Managers
{
    internal sealed class TokenManager : ITokenManager
    {
        private readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EnigmaVault", "token.data");
        private static readonly byte[] s_entropy = Encoding.Unicode.GetBytes("3yl0uJIFfN7jI432aUJe23wrFXWhygCQ");

        public void SaveTokens(AccessData tokens)
        {
            string jsonString = JsonSerializer.Serialize(tokens);
            byte[] tokenBytes = Encoding.UTF8.GetBytes(jsonString);
            byte[] encryptedBytes = ProtectedData.Protect(tokenBytes, s_entropy, DataProtectionScope.CurrentUser);
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            File.WriteAllBytes(_filePath, encryptedBytes);
        }

        public Maybe<AccessData> GetTokens()
        {
            if (!File.Exists(_filePath))
                return null;

            try
            {
                byte[] encryptedBytes = File.ReadAllBytes(_filePath);
                byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, s_entropy, DataProtectionScope.CurrentUser);
                string jsonString = Encoding.UTF8.GetString(decryptedBytes);
                var tokenData = JsonSerializer.Deserialize<AccessData>(jsonString);

                if (tokenData == null || string.IsNullOrEmpty(tokenData.AccessToken))
                    return null;

                return tokenData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void ClearTokens()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
    }
}
