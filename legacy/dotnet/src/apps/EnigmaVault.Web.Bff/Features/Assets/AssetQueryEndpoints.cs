using Crossdyne.Toolkit.Results;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.AssetsService.Clients;
using Shared.Contracts.AssetsService.Responses;
using Shared.Contracts.FileService.Clients;
using Shared.Contracts.FileService.Requests;
using Shared.Contracts.FileService.Responses;
using Shared.Web.Extensions;

namespace EnigmaVault.Web.Bff.Features.Assets
{
    public static class AssetQueryEndpoints
    {
        public static void MapAssetQueryEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("api/v1/asset", async (
                [FromServices] IAssetService assetClient, 
                [FromServices] IFileService fileClient) =>
            {
                Result<List<IconMetadataResponse>> assetsMetadataResponse = await assetClient.GetFilesMetadata();

                if (assetsMetadataResponse.IsFailure)
                    return assetsMetadataResponse.Errors.MapToMinimalApiResult();

                List<IconMetadataResponse> s3KeysResponse = assetsMetadataResponse.Value;

                if (s3KeysResponse == null || s3KeysResponse.Count == 0)
                    return Results.Ok();

                Result<BatchUrlResponse> urlsResponseResult = await fileClient.GetUrls(new BatchUrlRequest([.. s3KeysResponse.Select(x => new FileRequest(x.S3Key.Bucket, x.S3Key.FolderPath, x.S3Key.Name))], null));
    
                if (urlsResponseResult.IsFailure)
                    return urlsResponseResult.Errors.MapToMinimalApiResult();

                BatchUrlResponse urlResponse = urlsResponseResult.Value;

                List<AssetUrlResponse> response = [];

                foreach (var url in urlResponse.Urls)
                {
                    IconMetadataResponse? s3Key = s3KeysResponse.FirstOrDefault(x => x.S3Key.Name == url.Key);

                    if (s3Key == null)
                        continue;

                    response.Add(new AssetUrlResponse(s3Key.AssetId, s3Key.AssetName, url.Url, s3Key.CategoryId, s3Key.ProjectIds, s3Key.IsPublic));
                }

                return Results.Ok(response);
            });
        }
    }
}