using EnigmaVault.Web.Bff.Constants;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SecretService.Clients;
using Shared.Contracts.SecretService.Requests;
using Shared.Web.Extensions;

namespace EnigmaVault.Web.Bff.Features.Tags
{
    public static class TagCommandEndpoints
    {
        public static void MapTagEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapPost(UrlRouteConstants.TagBaseUrl, async ([FromBody] CreateTagRequest request, [FromServices] ITagService service) 
                => await service.CreateAsync(request).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPatch(UrlRouteConstants.TagBaseUrl, async ([FromBody] UpdateTagRequest request, [FromServices] ITagService service) 
                => await service.UpdateAsync(request).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapDelete($"{UrlRouteConstants.TagBaseUrl}/{{id}}", async ([FromRoute] string id, [FromServices] ITagService service) 
                => await service.DeleteAsync(id).MapErrorOrNoContentAsync()).RequireAuthorization();
        }
    }
}