using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Queries.GetDetails
{
    public sealed class GetEncryptedDetailsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetEncryptedDetailsQuery, Result<string>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result<string>> Handle(GetEncryptedDetailsQuery request, CancellationToken cancellationToken)
        {
            var vault = await _context.Set<VaultItem>().FirstOrDefaultAsync(v => v.UserId == request.UserId && v.Id == request.VaultItemId, cancellationToken);

            if (vault is null)
                return new Error(ErrorCode.NotFound, $"Элемент {request.VaultItemId} не был найден");

            return vault.EncryptedDetails.Value;
        }
    }
}