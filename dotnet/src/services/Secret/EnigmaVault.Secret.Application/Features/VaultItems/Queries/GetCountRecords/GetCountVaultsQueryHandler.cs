using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Queries.GetCountRecords
{
    public sealed class GetCountVaultsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetCountVaultsQuery, Result<int>>
    {
        public async Task<Result<int>> Handle(GetCountVaultsQuery request, CancellationToken cancellationToken)
            => await context.Set<VaultItem>().Where(v => v.UserId == request.UserId).CountAsync(cancellationToken);
    }
}