using Microsoft.AspNetCore.Mvc;

namespace Demo03.Requests.Api.Middlewares
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var correlationId = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString()
                          ?? context.TraceIdentifier;

                _logger.LogError(ex, "Unhandled exception. CorrelationId={CorrelationId}", correlationId);

                context.Response.ContentType = "application/json";
                context.Response.Headers[CorrelationIdMiddleware.HeaderName] = correlationId;

                var problem = ex switch
                {
                    ArgumentException => CreateProblem(
                        context,
                        StatusCodes.Status400BadRequest,
                        "Bad Request",
                        ex.Message),

                    InvalidOperationException => CreateProblem(
                        context,
                        StatusCodes.Status404NotFound,
                        "Resource Not Found",
                        ex.Message),

                    _ => CreateProblem(
                        context,
                        StatusCodes.Status500InternalServerError,
                        "Internal Server Error",
                        "An unexpected error occurred")
                };

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(problem);
            }
        }

        private static ProblemDetails CreateProblem(
            HttpContext context,
            int status,
            string title,
            string detail)
        {
            return new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path               
            };
        }
    }
}
