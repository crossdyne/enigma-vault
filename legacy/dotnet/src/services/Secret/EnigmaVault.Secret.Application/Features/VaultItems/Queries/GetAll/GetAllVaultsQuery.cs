using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Queries.GetAll
{
    public sealed record GetAllVaultsQuery(Guid UserId) : IRequest<Result<List<EncryptedVaultResponse>>>;
}