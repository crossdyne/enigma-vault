namespace EnigmaVault.Web.Bff.Models
{
    public sealed class UserSession(string sessionId, string encryptedAccessToken, string encryptedRefreshToken, DateTime accessTokenExpiresAt, string userId, string login)
    {
        public string SessionId { get; set; } = sessionId;
        public string EncryptedAccessToken { get; set; } = encryptedAccessToken;
        public string EncryptedRefreshToken { get; set; } = encryptedRefreshToken;
        public DateTime AccessTokenExpiresAt { get; set; } = accessTokenExpiresAt;
        public string UserId { get; set; } = userId;
        public string Login { get; set; } = login;
    }
}