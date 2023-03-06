using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FebSystem.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> logger;
        private readonly IHostEnvironment hostEnvironmentenv;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IHostEnvironment hostEnvironmentenv)
        {
            this.logger = logger;
            this.hostEnvironmentenv = hostEnvironmentenv;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            Exception exception= context.Exception;
            logger.LogError(exception, "there have some unhandled exception");
            string message;
            message = "unhandled exception";
            if (hostEnvironmentenv.IsDevelopment())
            {
                message = exception.Message;
            }
            ObjectResult result = new ObjectResult(new { code = 500, msg = message });
            result.StatusCode= 500;
            context.Result = result;
            context.ExceptionHandled= true;
            return Task.CompletedTask;
        }
    }
}
