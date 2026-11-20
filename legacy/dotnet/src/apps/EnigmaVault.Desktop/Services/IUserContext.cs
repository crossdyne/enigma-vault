using EnigmaVault.Desktop.Models;

namespace EnigmaVault.Desktop.Services
{
    public interface IUserContext
    {
        string AccessToken { get; }
        string Id { get; }
        string Login { get; }
        string RefreshToken { get; }
        public byte[] Dek { get; }

        void UpdateTokens(AccessData accessData);
        void UpdateUserInfo(UserInfo info);
        void UpdateDek(byte[] dek);
    }
}