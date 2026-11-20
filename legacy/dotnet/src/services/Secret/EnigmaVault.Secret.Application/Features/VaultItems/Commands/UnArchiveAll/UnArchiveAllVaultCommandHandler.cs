using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Commands.UnArchiveAll
{
    public sealed class UnArchiveAllVaultCommandHandler(
        IApplicationDbContext context, 
        IUnitOfWork unitOfWork) : IRequestHandler<UnArchiveAllVaultCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(UnArchiveAllVaultCommand request, CancellationToken cancellationToken)
        {
            var vaults = await context.Set<VaultItem>().Where(vi => vi.UserId == request.UserId && vi.IsArchive).ToListAsync(cancellationToken);

            foreach (var vault in vaults)
                vault.SetArchive(false);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}