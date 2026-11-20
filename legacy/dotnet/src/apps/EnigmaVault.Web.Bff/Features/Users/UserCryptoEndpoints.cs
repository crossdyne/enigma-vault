using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.UserManagementService.Clients;
using Shared.Web.Extensions;

namespace EnigmaVault.Web.Bff.Features.Users
{
    public static class UserCryptoEndpoints
    {
        public static void MapUserCryptoEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("api/v1/private/dek", async ([FromServices] IUserManagementService service) 
                => await service.GetDek().MapErrorOrOkAsync()).RequireAuthorization();
        }
    }
}