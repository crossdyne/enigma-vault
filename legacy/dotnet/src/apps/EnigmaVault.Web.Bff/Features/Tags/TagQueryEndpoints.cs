using EnigmaVault.Web.Bff.Constants;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SecretService.Clients;
using Shared.Web.Extensions;

namespace EnigmaVault.Web.Bff.Features.Tags
{
    public static class TagQueryEndpoints
    {
        public static void MapTagEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(UrlRouteConstants.TagBaseUrl, async ([FromServices] ITagService service) 
                => await service.GetAll().MapErrorOrOkAsync()).RequireAuthorization();
        }
    }
}