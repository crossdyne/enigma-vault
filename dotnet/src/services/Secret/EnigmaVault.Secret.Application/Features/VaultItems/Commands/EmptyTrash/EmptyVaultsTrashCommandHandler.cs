using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.EmptyTrash
{
    public sealed record EmptyVaultsTrashCommandHandler(
        IApplicationDbContext context,
        IUnitOfWork unitOfWork,
        IVaultItemRepository vaultItemRepository) : IRequestHandler<EmptyVaultsTrashCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;

        public async Task<Result<Unit>> Handle(EmptyVaultsTrashCommand request, CancellationToken cancellationToken)
        {
            var vaults = await _context.Set<VaultItem>().Where(vi => vi.UserId == request.UserId && vi.IsInTrash).ToListAsync(cancellationToken);

            foreach (var vault in vaults)
                _vaultItemRepository.Remove(vault);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
