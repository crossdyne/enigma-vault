namespace Shared.Contracts.SecretService.Responses
{
    public sealed record EncryptedVaultResponse(string Id, string Type, DateTime DateAdded, DateTime? DateUpdate, DateTime? DeletedAt, bool IsFavorite, bool IsArchive, bool IsInTrash, string EncryptedOverview, string EncryptedDetails, List<string> TagsIds, string IconId);
}