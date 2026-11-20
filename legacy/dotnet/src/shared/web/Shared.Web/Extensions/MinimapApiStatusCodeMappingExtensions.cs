using Crossdyne.Toolkit.Results;
using Microsoft.AspNetCore.Http;

namespace Shared.Web.Extensions
{
    public static class MinimapApiStatusCodeMappingExtensions
    {
        #region Ok

        public static IResult MapErrorOrOk(this Result result)
        {
            if (result.IsFailure)
                return result.Errors.MapToMinimalApiResult();

            return Results.Ok();
        }

        public static IResult MapErrorOrOk<TResult>(this Result<TResult> result)
        {
            if (result.IsFailure)
                return result.Errors.MapToMinimalApiResult();

            return Results.Ok(result.Value);
        }

        public static async Task<IResult> MapErrorOrOkAsync(this Task<Result> asyncResult)
        {
            var result = await asyncResult;

            return result.MapErrorOrOk();
        }

        public static async Task<IResult> MapErrorOrOkAsync<TResult>(this Task<Result<TResult>> asyncResult)
        {
            var result = await asyncResult;

            return result.MapErrorOrOk<TResult>();
        }

        #endregion

        #region NoContent

        public static IResult MapErrorOrNoContent(this Result result)
        {
            if (result.IsFailure)
                return result.Errors.MapToMinimalApiResult();

            return Results.Ok();
        }

        public static async Task<IResult> MapErrorOrNoContentAsync(this Task<Result> asyncResult)
        {
            var result = await asyncResult;

            return result.MapErrorOrNoContent();
        }

        
        public static IResult MapErrorOrNoContent<TResult>(this Result<TResult> result)
        {
            if (result.IsFailure)
                return result.Errors.MapToMinimalApiResult();

            return Results.Ok();
        }

        public static async Task<IResult> MapErrorOrNoContentAsync<TResult>(this Task<Result<TResult>> asyncResult)
        {
            var result = await asyncResult;

            return result.MapErrorOrNoContent();
        }

        #endregion

    }
}