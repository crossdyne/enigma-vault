using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.UnArchive
{
    public sealed record UnArchiveVaultCommand(Guid VaultItemId, Guid UserId) : IRequest<Result>;
}