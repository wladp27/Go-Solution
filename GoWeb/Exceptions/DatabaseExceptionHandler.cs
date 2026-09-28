using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoWeb.Exceptions
{
    public class DatabaseExceptionHandler : IExceptionHandler
    {

        private readonly ILogger<DatabaseExceptionHandler> _logger;

        public DatabaseExceptionHandler(ILogger<DatabaseExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not DbUpdateException dbException)
            {
                return false; 
            }

            _logger.LogError(dbException, "Ошибка при сохранении в базу данных.");

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict, 
                Title = "Ошибка базы данных",
                Detail = "Не удалось сохранить данные. Возможно, такая запись уже существует.",
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);


            return true;
        }
    }
}
