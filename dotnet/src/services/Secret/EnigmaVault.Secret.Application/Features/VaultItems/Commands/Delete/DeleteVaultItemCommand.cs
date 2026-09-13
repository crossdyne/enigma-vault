using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.Delete
{
    public sealed record DeleteVaultItemCommand(Guid UserId, Guid VaultItemId) : IRequest<Result<Unit>>;
}