namespace Shared.Contracts.SecretService.Requests
{
    public sealed record UpdateTagRequest(string Id, string Name, string Color);
}