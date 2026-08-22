using Microsoft.AspNetCore.Mvc;

namespace Cafe_Management.CustomMiddleWares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate next, ILogger<ExceptionHandlerMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                await HandelNotFoundAsync(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error occurred");
                var problem = new ProblemDetails()
                {
                    Title = "Internal Server Error occurred ",
                    Detail = ex.Message,
                    Instance = httpContext.Request.Path,
                    Status = StatusCodes.Status500InternalServerError
                };
                httpContext.Response.StatusCode = problem.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(problem);
            }
        }

        private static async Task HandelNotFoundAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound && !httpContext.Response.HasStarted)
            {
                var response = new ProblemDetails()
                {
                    Title = "Error while Processing The HTTP Request - EndPoint Not Found ",
                    Detail = $"EndPoint {httpContext.Request.Path} not found ",
                    Status = StatusCodes.Status404NotFound,
                    Instance = httpContext.Request.Path
                };
                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
