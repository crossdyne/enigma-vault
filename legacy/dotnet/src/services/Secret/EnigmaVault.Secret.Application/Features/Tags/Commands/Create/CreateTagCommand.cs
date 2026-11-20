using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Features.Validators;
using MediatR;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Application.Features.Tags.Commands.Create
{
    public sealed record CreateTagCommand(Guid UserId, string Name, string Color) : IRequest<Result<CreateTagResponse>>,
        IMustHasUserId,
        IHasName;
}