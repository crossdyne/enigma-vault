namespace Shared.Contracts.SecretService.Requests
{
    public sealed record UpdateTagsRequest(List<string> TagIds);
}