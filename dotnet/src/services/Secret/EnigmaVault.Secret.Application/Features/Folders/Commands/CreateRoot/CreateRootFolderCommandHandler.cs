using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Domain.Models;
using MediatR;

namespace EnigmaVault.Secret.Application.Features.Folders.Commands.CreateRoot
{
    internal sealed class CreateRootFolderCommandHandler(
        IFolderRepository repository, 
        IUnitOfWork unitOfWork) : IRequestHandler<CreateRootFolderCommand, Result<Unit>>
    {
        private readonly IFolderRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(CreateRootFolderCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.Exist(request.Name, request.UserId, null, cancellationToken))
                return new Error(ErrorCode.Conflict, "Папка с данными именем уже есть.");

            var folder = Folder.CreateRoot(request.UserId, request.Name, request.Color);

            await _repository.AddAsync(folder, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}