using Crossdyne.Toolkit.Results;

namespace Shared.Kernel.Errors
{
    public static class AppErrors
    {
        public static readonly ErrorCode ApiError = ErrorCode.Custom(nameof(ApiError), 10001);
        public static readonly ErrorCode Validation = ErrorCode.Custom(nameof(Validation), 10002);
        public static readonly ErrorCode Rule = ErrorCode.Custom(nameof(Rule), 10003);
        public static readonly ErrorCode RequestCancelled = ErrorCode.Custom(nameof(RequestCancelled), 10004);
    }
}