using FirmTracker_Server.Exceptions;

namespace FirmTracker_Server.Middleware
{
    public class ErrorHandling : IMiddleware
    {
        private readonly ILogger Logger;

        public ErrorHandling(ILogger<ErrorHandling> logger)
        {
            Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (WrongUserOrPasswordException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (PermissionException ex)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (NoResultsException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Wystąpił nieoczekiwany błąd.");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync("Wystąpił nieoczekiwany błąd.");
            }
        }
    }
}
