namespace EnigmaVault.Web.Bff.Services
{
    public interface IJwtReadService
    {
        JwtExtractedData ExtractData(string token);
    }
}