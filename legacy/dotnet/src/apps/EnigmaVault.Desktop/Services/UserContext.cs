using EnigmaVault.Desktop.Models;

namespace EnigmaVault.Desktop.Services
{
    public sealed class UserContext : IUserContext
    {
        public string Id { get; private set; } = null!;
        public string Login { get; private set; } = null!;

        public string AccessToken { get; private set; } = null!;
        public string RefreshToken { get; private set; } = null!;
        public byte[] Dek { get; private set; } = null!;

        public void UpdateTokens(AccessData accessData)
        {
            AccessToken = accessData.AccessToken;
            RefreshToken = accessData.RefreshToken;
        }

        public void UpdateUserInfo(UserInfo info)
        {
            Id = info.Id;
            Login = info.Login;
        }

        public void UpdateDek(byte[] dek)
        {
            Dek = dek;
        }
    }
}