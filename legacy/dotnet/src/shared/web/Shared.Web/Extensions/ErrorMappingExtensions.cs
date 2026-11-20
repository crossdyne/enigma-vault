using Crossdyne.Toolkit.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Web.Extensions
{
    public static class ErrorMappingExtensions
    {
        public static IActionResult MapActionResult(this Controller controller, IReadOnlyList<Error> errors)
        {
            if (errors == null || errors.Count == 0)
                return controller.StatusCode(StatusCodes.Status500InternalServerError);

            var statusCode = errors[0].Code.Name switch
            {
                nameof(ErrorCode.NotFound) => StatusCodes.Status404NotFound,
                
                nameof(ErrorCode.Save) or
                nameof(ErrorCode.Empty) or 
                nameof(ErrorCode.Create) => StatusCodes.Status500InternalServerError,

                nameof(ErrorCode.Conflict) => StatusCodes.Status409Conflict,

                nameof(ErrorCode.Unauthorized) => StatusCodes.Status401Unauthorized,

                _ => StatusCodes.Status400BadRequest
            };

            return controller.StatusCode(statusCode, errors);
        }
    }
}