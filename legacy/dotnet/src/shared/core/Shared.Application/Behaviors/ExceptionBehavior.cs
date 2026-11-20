using Crossdyne.Toolkit.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;

namespace Shared.Application.Behaviors
{
    public sealed class ExceptionBehavior<TRequest, TResponse>(ILogger<ExceptionBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result, IResultWithFactory<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next(cancellationToken);
            }
            catch (DomainException ex)
            {
                logger.LogWarning(ex, "Доменная ошибка при обработке {RequestType}", typeof(TRequest).Name);

                return TResponse.CreateFailure(new[] { ex.Error }); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Необработанное исключение при обработке запроса {RequestType}", typeof(TRequest).Name);

                var error = new Error(ErrorCode.Server, "Ошибка на стороне сервера");
                return TResponse.CreateFailure(new[] { error });
            }
        }
    }
}