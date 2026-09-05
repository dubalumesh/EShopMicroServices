using MediatR;
using BuildingBlocks.CQRS;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Diagnostics;


namespace BuildingBlocks.Behavior
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {RequestName} with content: {@Request}", typeof(TRequest).Name, request);

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();
            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > 3)
            {
                logger.LogWarning("[Performance] The request { @Request} took {@timeTaken}", typeof(TResponse).Name, timeTaken.Seconds);


            }

            logger.LogInformation("Handled {RequestName} with response: {@Response}", typeof(TRequest).Name, response);
            return response;
        }
    }
}
