using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.MoveToTrash
{
    public sealed record MoveVaultToTrashCommand(Guid UserId, Guid VaultItemId) : IRequest<Result<DateTime>>;
}