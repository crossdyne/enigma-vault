using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Domain.ValueObjects.Common;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Application.Features.Tags.Commands.Update
{
    internal sealed class UpdateTagCommandHandler(
        ITagRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateTagCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITagRepository _repository = repository;

        public async Task<Result<Unit>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var maybeTag = await _repository.GetAsync(request.Id, request.UserId, token: cancellationToken);

            if (maybeTag.IsNone)
                return new Error(ErrorCode.NotFound, $"Тэг {request.Id} не был найден");

            maybeTag.Value.UpdateName(TagName.Create(request.Name));
            maybeTag.Value.UpdateColor(Color.FromHex(request.Color));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}