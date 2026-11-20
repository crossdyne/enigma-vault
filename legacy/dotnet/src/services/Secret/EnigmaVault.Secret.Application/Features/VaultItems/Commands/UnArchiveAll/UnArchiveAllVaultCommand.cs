using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.UnArchiveAll
{
    public sealed record UnArchiveAllVaultCommand(Guid UserId) : IRequest<Result<Unit>>;
}