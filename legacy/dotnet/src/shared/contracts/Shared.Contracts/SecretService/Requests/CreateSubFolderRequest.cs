namespace Shared.Contracts.SecretService.Requests
{
    public sealed record CreateSubFolderRequest(string ParentFolderId, string Name, string Color);
}