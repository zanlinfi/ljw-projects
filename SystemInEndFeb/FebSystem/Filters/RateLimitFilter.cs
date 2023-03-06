using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace FebSystem.Filters
{
    /// <summary>
    /// only one customer visit from one IP in one second
    /// </summary>
    public class RateLimitFilter : IAsyncActionFilter
    {
        private readonly IMemoryCache memCache;

        public RateLimitFilter(IMemoryCache memCache)
        {
            this.memCache = memCache;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string removeIP = context.HttpContext.Connection.RemoteIpAddress!.ToString();
            string cacheKey = $"LastVisitTick_{removeIP}";
            long? lastTick = memCache.Get<long?>(cacheKey);
            if (lastTick == null || Environment.TickCount64 - lastTick > 10)
            {
                memCache.Set(cacheKey, Environment.TickCount64, TimeSpan.FromSeconds(10));
                return next();
            }
            //else
            //{
            context.Result = new ContentResult { StatusCode = 429, Content="per second once only"};
            return Task.CompletedTask;
            //}
        }
    }
}
