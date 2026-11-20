using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Application.Features.Folders.Commands.Delete
{
    internal sealed class DeleteFolderCommandHandler(
        IApplicationDbContext context,
        IFolderRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteFolderCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IFolderRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
        {
            var maybeFolder = await _repository.GetAsync(request.Id, request.UserId, cancellationToken);

            if (maybeFolder.IsNone)
                return new Error(ErrorCode.NotFound, $"Папка {request.Id} не была найдена");

            var children = await _context.Set<Folder>().Where(x => x.ParentFolderId == maybeFolder.Value.Id).ToListAsync(cancellationToken);

            _repository.Remove(maybeFolder.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}