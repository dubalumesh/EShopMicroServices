

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;


namespace BuildingBlocks.Middlewares
{
    public class ValidationExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is FluentValidation.ValidationException validationException)
            {
                httpContext.Response.StatusCode = 400;

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Message = "Validation failed",
                    Errors = validationException.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList()
                });
                return true;
            }
            return false;
        }
    }
}
