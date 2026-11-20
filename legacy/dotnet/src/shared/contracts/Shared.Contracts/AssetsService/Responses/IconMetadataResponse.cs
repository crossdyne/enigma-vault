namespace Shared.Contracts.AssetsService.Responses
{
    public sealed record IconMetadataResponse(string AssetId, string AssetName, S3KeyResponse S3Key, string CategoryId, List<string> ProjectIds, bool IsPublic);
}