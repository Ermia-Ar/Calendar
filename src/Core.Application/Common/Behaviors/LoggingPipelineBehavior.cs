using MediatR;
using SharedKernel.Loggers.Abstraction;
using SharedKernel.ResponseResult;

namespace Core.Application.Common.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultResponse
{
    private readonly ILoggerService _logger;

    public LoggingPipelineBehavior(ILoggerService logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {

        _logger.LogInformation(
            " --Starting request {@RequestName}, {@DateTimeUtc} , {@Id}",
            typeof(TRequest).Name,
            DateTime.UtcNow,
            Guid.NewGuid()
            );

        var result = await next();

        if (result.IsFailed)
        {
            _logger.LogError(
                "--Request failure {@RequestName}, {@Error}, {@DateTimeUtc}",
                typeof(TRequest).Name,
                result.Message,
                DateTime.UtcNow);
        }

        _logger.LogInformation(
            "--Completed request {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        return result;
    }
}

