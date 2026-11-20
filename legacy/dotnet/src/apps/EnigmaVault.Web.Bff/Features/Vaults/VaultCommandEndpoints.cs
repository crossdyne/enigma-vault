using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SecretService.Clients;
using Shared.Contracts.SecretService.Requests;
using Shared.Web.Extensions;

namespace EnigmaVault.Web.Bff.Features.Vaults
{
    public static class VaultCommandEndpoints
    {
        public static void MapVaultCommandEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("api/v1/vault", async ([FromBody] CreateVaultItemRequest request, [FromServices] IVaultService service) 
                => await service.CreateAsync(request).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPut("api/v1/vault", async ([FromBody] UpdateVaultItemRequest request, [FromServices] IVaultService service) 
                => await service.UpdateAsync(request).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPatch("api/v1/vault/trash/{id}", async ([FromRoute] string id, [FromServices] IVaultService service) 
                => await service.MoveToTrashAsync(id).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPatch("api/v1/vault/restore/{id}", async ([FromRoute] string id, [FromServices] IVaultService service) 
                => await service.RestoreFromTrashAsync(id).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapDelete("api/v1/vault/{id}", async ([FromRoute] string id, [FromServices] IVaultService service) 
                => await service.DeleteAsync(id).MapErrorOrNoContentAsync()).RequireAuthorization();

            builder.MapPatch("api/v1/vault/trash/empty", async ([FromServices] IVaultService service) 
                => await service.EmptyTrashAsync().MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPatch("api/v1/vault/restore/all", async ([FromServices] IVaultService service) 
                => await service.RestoreAllFromTrashAsync().MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPatch("api/v1/vault/zip/{id}", async ([FromRoute] string id, [FromServices] IVaultService service) 
                => await service.ArchiveAsync(id).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPatch("api/v1/vault/unzip/{id}", async ([FromRoute] string id, [FromServices] IVaultService service) 
                => await service.UnArchiveAsync(id).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPatch("api/v1/vault/unzip/all", async ([FromServices] IVaultService service) 
                => await service.RestoreAllFromArchiveAsync().MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPatch("api/v1/vault/change/{vaultId}/icon/{iconId}", async ([FromRoute] string vaultId, [FromRoute] string iconId, [FromServices] IVaultService service)
                 => await service.ChangeIcon(vaultId, iconId).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPatch("api/v1/vault/tags/{vaultId}", async ([FromRoute] string vaultId, [FromBody] UpdateTagsRequest request, [FromServices] IVaultService service) 
                => await service.UpdateTagsAsync(vaultId, request).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPatch("api/v1/vault/favorite/{vaultId}", async ([FromRoute] string vaultId, [FromServices] IVaultService service) 
                => await service.AddToFavoritesAsync(vaultId).MapErrorOrOkAsync()).RequireAuthorization();

             builder.MapPatch("api/v1/vault/unfavorite/{vaultId}", async ([FromRoute] string vaultId, [FromServices] IVaultService service) 
                => await service.RemoveFromFavoritesAsync(vaultId).MapErrorOrOkAsync()).RequireAuthorization();
        }
    }
}