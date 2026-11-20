using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.UpdateTags
{
    public sealed record UpdateTagsCommand(Guid UserId, Guid VaultId, List<Guid> TagIds) : IRequest<Result<DateTime>>;
}