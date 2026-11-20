using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.RestoreFromTrash
{
    public sealed record RestoreVaultFromTrashCommand(Guid UserId, Guid VaultItemId) : IRequest<Result<Unit>>;
}