namespace Shared.Contracts.FileService.Responses
{
    public sealed record AssetUrlResponse(string AssetId, string AssetName, string Url, string CategoryId, List<string> ProjectIds, bool IsPublic);
}