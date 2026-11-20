namespace Shared.Contracts.SecretService.Responses
{
    public sealed record IconResponse(string Id, string? UserId, string SvgCode, string IconName, string IconCategoryId);
}