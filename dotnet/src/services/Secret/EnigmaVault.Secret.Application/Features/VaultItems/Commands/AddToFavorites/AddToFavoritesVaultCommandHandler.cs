using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.AddToFavorites
{
    public sealed class AddToFavoritesVaultCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<AddToFavoritesVaultCommand, Result<Unit>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(AddToFavoritesVaultCommand request, CancellationToken cancellationToken)
        {
            var maybeVault = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

            if (maybeVault.IsNone)
                return new Error(ErrorCode.NotFound, $"Элемента {request.VaultItemId} не был найден");

            var vault = maybeVault.Value;

            vault.SetFavorite(true);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}