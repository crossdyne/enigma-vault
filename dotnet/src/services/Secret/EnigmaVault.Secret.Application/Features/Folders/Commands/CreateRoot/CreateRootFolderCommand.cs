using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Features.Folders.Validators;
using EnigmaVault.Secret.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Secret.Application.Features.Folders.Commands.CreateRoot
{
    public sealed record CreateRootFolderCommand(Guid UserId, string Name, string Color) : IRequest<Result<Unit>>,
        IMustHasUserId,
        IHasName,
        IHasHexColor;
}