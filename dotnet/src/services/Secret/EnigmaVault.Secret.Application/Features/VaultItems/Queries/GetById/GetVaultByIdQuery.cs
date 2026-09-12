using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Application.Features.VaultItems.Queries.GetById
{
    public sealed record GetVaultByIdQuery(Guid Id, Guid UserId) : IRequest<Result<EncryptedVaultResponse>>;
}