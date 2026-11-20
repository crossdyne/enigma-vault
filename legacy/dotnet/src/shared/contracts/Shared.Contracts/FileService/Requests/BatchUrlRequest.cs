using Shared.Contracts.FileService.Responses;

namespace Shared.Contracts.FileService.Requests
{
    public sealed record BatchUrlRequest(List<FileRequest> Files, int? Expires);
}