using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using MediatR;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.UnArchive
{
    public sealed class UnArchiveVaultCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<UnArchiveVaultCommand, Result>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(UnArchiveVaultCommand request, CancellationToken cancellationToken)
        {
            var mayBe = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

            if (mayBe.IsNone)
                return Result.Failure(new Error(ErrorCode.NotFound, $"Данный элемент {request.VaultItemId} не был найден"));

            mayBe.Value.SetArchive(false);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}