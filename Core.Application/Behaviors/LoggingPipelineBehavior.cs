using MediatR;
using Serilog;

namespace Application.Behaviors;


public class LoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggingPipelineBehavior(ILogger logger)
    {
        _logger = logger.ForContext<LoggingPipelineBehavior<TRequest, TResponse>>();
    }


    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {

        _logger.Information(
            " --Starting request {@RequestName}, {@DateTimeUtc} , {@Id}",
            typeof(TRequest).Name,
            DateTime.UtcNow,
            Guid.NewGuid()
            );

        var result = await next();

        //if (!result.Succeeded)
        //{
        //    _logger.Error(
        //        "--Request failure {@RequestName}, {@Error}, {@DateTimeUtc}",
        //        typeof(TRequest).Name,
        //        result.Message,
        //        DateTime.UtcNow);
        //}

        _logger.Information(
            "--Completed request {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        return result;
    }
}

