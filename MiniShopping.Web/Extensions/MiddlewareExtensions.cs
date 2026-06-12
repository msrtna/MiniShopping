using MiniShopping.Web.Middlewares;

namespace MiniShopping.Web.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder
        UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
