using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Application.Features.Tags.Commands.Delete
{
    public sealed record DeleteTagCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>;
}