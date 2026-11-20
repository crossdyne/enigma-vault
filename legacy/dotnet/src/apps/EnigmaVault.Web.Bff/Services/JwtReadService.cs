using System.IdentityModel.Tokens.Jwt;

namespace EnigmaVault.Web.Bff.Services
{
    public record JwtExtractedData(string UserId, string Login, DateTime ExpiredTime);

    public sealed class JwtReadService : IJwtReadService
    {
        private static readonly JwtSecurityTokenHandler _handler = new();

        public JwtExtractedData ExtractData(string token)
        {
            var jwt = _handler.ReadJwtToken(token);

            var userId = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var login = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jwt.Claims.First(c => c.Type == "exp").Value)).UtcDateTime;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(login))
                throw new InvalidOperationException("В токене доступа отсутствуют необходимые поля (sub/name).");

            return new JwtExtractedData(userId, login, exp);
        }
    }
}