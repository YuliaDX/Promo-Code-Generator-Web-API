using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PromocodeFactoryProject
{
    public class ApplicationVersionMiddleware
    {
        private readonly RequestDelegate _next;
        public ApplicationVersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.ToLower().EndsWith("/api/version"))
            {
                var version = typeof(ApplicationVersionMiddleware).Assembly.GetName().Version.ToString();
                await context.Response.WriteAsync($"The project version: {version}");
            }
            else
                await _next(context);
        }
    }
}