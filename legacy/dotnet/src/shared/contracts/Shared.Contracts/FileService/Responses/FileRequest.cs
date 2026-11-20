namespace Shared.Contracts.FileService.Responses
{
    public sealed record FileRequest(string Bucket, string Folder, string Key);
}