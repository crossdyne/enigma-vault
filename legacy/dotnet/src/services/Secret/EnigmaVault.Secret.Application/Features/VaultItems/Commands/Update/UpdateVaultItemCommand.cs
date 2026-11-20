using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.Update
{
    public sealed record UpdateVaultItemCommand(Guid UserId, Guid VaultItemId, Guid IconId, string EncryptedOverview, string EncryptedDetails, int CryptoVersion) : IRequest<Result<string>>;
}