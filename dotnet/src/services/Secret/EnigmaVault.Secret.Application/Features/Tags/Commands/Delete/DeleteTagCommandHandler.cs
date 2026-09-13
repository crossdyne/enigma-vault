using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Application.Features.Tags.Commands.Delete
{
    internal sealed class DeleteTagCommandHandler(
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteTagCommand, Result<Unit>>
    {
        private readonly ITagRepository _tagRepository = tagRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var maybeTag = await _tagRepository.GetAsync(request.Id, request.UserId, token: cancellationToken);

            if (maybeTag.IsNone)
                return new Error(ErrorCode.NotFound, $"Тэг {request.Id} не был найден.");

            _tagRepository.Remove(maybeTag.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}