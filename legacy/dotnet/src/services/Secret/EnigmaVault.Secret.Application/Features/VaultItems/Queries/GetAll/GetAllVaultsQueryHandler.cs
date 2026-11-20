using AutoMapper;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Queries.GetAll
{
    public sealed class GetAllVaultsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllVaultsQuery, Result<List<EncryptedVaultResponse>>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<EncryptedVaultResponse>>> Handle(GetAllVaultsQuery request, CancellationToken cancellationToken)
        {
            var vaultItems = await _context.Set<VaultItem>()
                .AsNoTracking()
                .Include(vi => vi.Tags)
                .Where(v => v.UserId == request.UserId)
                .Select(x => new
                {
                    x.Id,
                    x.VaultType,
                    x.DateAdded,
                    x.DateUpdated,
                    x.DeletedAt,
                    x.IsFavorite,
                    x.IsArchive,
                    x.IsInTrash,
                    x.EncryptedOverview,
                    x.EncryptedDetails,
                    Tags = x.Tags,
                    x.IconId
                })
                .ToListAsync(cancellationToken);

            var response = vaultItems.Select(x => new EncryptedVaultResponse(
                x.Id.ToString(),
                x.VaultType.ToString(),
                x.DateAdded,
                x.DateUpdated,
                x.DeletedAt,
                x.IsFavorite,
                x.IsArchive,
                x.IsInTrash,
                x.EncryptedOverview,
                x.EncryptedDetails,
                [.. x.Tags.Select(vt => vt.TagId.ToString())],
                x.IconId.ToString()
            )).ToList();

            return response;
        }
    }
}