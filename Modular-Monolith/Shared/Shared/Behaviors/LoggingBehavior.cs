using System.Diagnostics;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private static readonly TimeSpan LongRunningThreshold = TimeSpan.FromSeconds(3);

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var requestId = Guid.NewGuid();

            using var _ = logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = requestId,
                ["RequestName"] = requestName
            });

            logger.LogInformation("[START] {RequestName} {RequestId} {@Request}", requestName, requestId, Serialize(request));

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await next();

                stopwatch.Stop();

                if (stopwatch.Elapsed > LongRunningThreshold)
                {
                    logger.LogWarning(
                        "[LONG RUNNING] {RequestName} {RequestId} took {ElapsedMilliseconds}ms",
                        requestName, requestId, stopwatch.ElapsedMilliseconds);
                }

                logger.LogInformation(
                    "[END] {RequestName} {RequestId} completed in {ElapsedMilliseconds}ms",
                    requestName, requestId, stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                logger.LogError(
                    ex,
                    "[ERROR] {RequestName} {RequestId} failed after {ElapsedMilliseconds}ms",
                    requestName, requestId, stopwatch.ElapsedMilliseconds);

                throw;
            }
        }

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = false,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };


        private static string Serialize(TRequest request)
        {
            try
            {
                return JsonSerializer.Serialize(request, SerializerOptions);
            }
            catch (Exception ex)
            {
                return $"<unable to serialize request: {ex.Message}>";
            }
        }
    }
}
