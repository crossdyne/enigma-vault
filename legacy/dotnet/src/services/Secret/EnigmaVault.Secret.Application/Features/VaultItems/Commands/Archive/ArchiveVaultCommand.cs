using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.Archive
{
    public sealed record ArchiveVaultCommand(Guid VaultItemId, Guid UserId) : IRequest<Result<Unit>>;
}