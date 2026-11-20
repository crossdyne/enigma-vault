namespace Shared.Contracts.SecretService.Requests
{
    public sealed record UpdateFolderRequest(string Id, string? ParentFolderId, string Name);
}