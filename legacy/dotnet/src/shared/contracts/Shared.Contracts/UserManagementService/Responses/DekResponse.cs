namespace Shared.Contracts.UserManagementService.Responses;

public sealed record DekResponse(string ClientSalt, string EncryptedDek, int CryptoVersion, string Login);