namespace Shared.Contracts.SecretService.Responses
{
    public sealed record FolderResponse(string Id, string UserId, string? ParentFolderId, string FolderName, string Color);
}