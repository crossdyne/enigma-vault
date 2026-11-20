using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Queries.GetById
{
    public sealed class GetVaultByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetVaultByIdQuery, Result<EncryptedVaultResponse>>
    {
        public async Task<Result<EncryptedVaultResponse>> Handle(GetVaultByIdQuery request, CancellationToken cancellationToken)
        {
            var vault = await context.Set<VaultItem>().AsNoTracking().FirstOrDefaultAsync(v => v.UserId == request.UserId && v.Id == request.Id);

            if (vault == null)
                return new Error(ErrorCode.NotFound, "Не найдено записи, возможно она уже удалена");

            return new EncryptedVaultResponse(
                vault.Id.ToString(), 
                vault.VaultType.ToString(), 
                vault.DateAdded, 
                vault.DateUpdated, 
                vault.DeletedAt, 
                vault.IsFavorite, 
                vault.IsArchive, 
                vault.IsInTrash, 
                vault.EncryptedOverview, 
                vault.EncryptedDetails, 
                [.. vault.Tags.Select(vt => vt.TagId.ToString())], 
                vault.IconId.ToString());
        }
    }
}