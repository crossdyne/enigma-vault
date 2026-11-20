namespace Shared.Contracts.FileService.Responses
{
    public sealed record BatchUrlResponse(string Status, int ExpiresIn, List<FileUrl> Urls, List<FileError> Errors, string Reason);
}