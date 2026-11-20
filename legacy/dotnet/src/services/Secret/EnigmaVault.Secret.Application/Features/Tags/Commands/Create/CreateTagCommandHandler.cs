using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Domain.Models;
using MediatR;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Application.Features.Tags.Commands.Create
{
    internal sealed class CreateTagCommandHandler(
        ITagRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateTagCommand, Result<CreateTagResponse>>
    {
        private readonly ITagRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<CreateTagResponse>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = Tag.Create(request.UserId, request.Name, request.Color);

            await _repository.AddAsync(tag, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateTagResponse(tag.Id.Value.ToString());
        }
    }
}
