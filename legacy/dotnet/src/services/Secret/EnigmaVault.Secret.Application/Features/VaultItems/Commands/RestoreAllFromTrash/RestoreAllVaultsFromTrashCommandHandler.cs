using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.RestoreAllFromTrash
{
    public sealed record RestoreAllVaultsFromTrashCommandHandler(
        IApplicationDbContext context,
        IUnitOfWork unitOfWork) : IRequestHandler<RestoreAllVaultsFromTrashCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(RestoreAllVaultsFromTrashCommand request, CancellationToken cancellationToken)
        {
            var vaults = await _context.Set<VaultItem>().Where(vi => vi.UserId == request.UserId && vi.IsInTrash).ToListAsync(cancellationToken);

            foreach (var vault in vaults)
                vault.SetInTrash(false);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}