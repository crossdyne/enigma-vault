using Crossdyne.Toolkit.Results;
using EnigmaVault.Web.Bff.Features.Overviews.Responses;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SecretService.Clients;
using Shared.Contracts.SecretService.Responses;
using Shared.Web.Extensions;

namespace EnigmaVault.Web.Bff.Features.Overviews
{
    public static class OverviewQueryEndpoints
    {
        public static void MapOverviewQueryEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("api/v1/overview/records/count", async ([FromServices] IVaultService vaultService) =>
            {
                Result<PasswordsCountRecordsResponse> passwordsCountResult = await vaultService.CountRecordsAsync();

                if (passwordsCountResult.IsFailure)
                    return passwordsCountResult.Errors.MapToMinimalApiResult();

                return Results.Ok(new CountRecordsResponse(passwordsCountResult.Value.Count));
            }).RequireAuthorization();
        }
    }
}