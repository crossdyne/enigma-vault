using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using MediatR;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.MoveToTrash
{
    public sealed class MoveVaultToTrashCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<MoveVaultToTrashCommand, Result<DateTime>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<DateTime>> Handle(MoveVaultToTrashCommand request, CancellationToken cancellationToken)
        {
            var maybeVault = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

            if (maybeVault.IsNone)
                return new Error(ErrorCode.NotFound, "Данные не найдены.");

            var vaultItem = maybeVault.Value;

            vaultItem.SetInTrash(true);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return vaultItem.DeletedAt!;
        }
    }
}
