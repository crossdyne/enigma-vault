using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnigmaVault.Secret.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static Result<ExtractData> ExtractCredentials(this Controller controller, ClaimsPrincipal user)
        {
            var extractData = new ExtractData();

            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                extractData.Result = controller.Unauthorized("User ID не найден в токене.");

            if (!Guid.TryParse(userIdString, out var userIdGuid))
                extractData.Result = controller.BadRequest("Не верный User ID формат.");

            extractData.UserId = userIdGuid;

            return Result<ExtractData>.Success(extractData);
        }
    }
}