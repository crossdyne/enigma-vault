using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SecretService.Clients;
using Shared.Web.Extensions;

namespace EnigmaVault.Web.Bff.Features.Vaults
{
    public static class VaultQueryEndpoints
    {
        public static void MapVaultQueryEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("api/v1/vault", async ([FromServices] IVaultService service) 
                => await service.GetAllAsync().MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapGet("api/v1/vault/{id}", async ([FromRoute] string id, [FromServices] IVaultService service) 
                => await service.GetById(id).MapErrorOrOkAsync()).RequireAuthorization();
        }
    }
}