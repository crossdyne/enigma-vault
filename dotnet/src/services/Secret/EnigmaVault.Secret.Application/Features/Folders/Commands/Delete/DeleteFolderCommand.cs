using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Secret.Application.Features.Folders.Commands.Delete
{
    public sealed record DeleteFolderCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>,
        IHasGuidId,
        IMustHasUserId;
}