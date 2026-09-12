using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Domain.Models;
using MediatR;

namespace EnigmaVault.Secret.Application.Features.Folders.Commands.CreateSubFolder
{
    internal sealed class CreateSubFolderCommandHandler(
        IFolderRepository repository, 
        IUnitOfWork unitOfWork) : IRequestHandler<CreateSubFolderCommand, Result<Unit>>
    {
        private readonly IFolderRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(CreateSubFolderCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.Exist(request.Name, request.UserId, request.ParentFolderId, cancellationToken))
                return new Error(ErrorCode.Conflict, "Папка с данными именем уже есть.");

            var folder = Folder.CreateSubfolder(request.UserId, request.ParentFolderId, request.Name, request.Color);

            await _repository.AddAsync(folder, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}