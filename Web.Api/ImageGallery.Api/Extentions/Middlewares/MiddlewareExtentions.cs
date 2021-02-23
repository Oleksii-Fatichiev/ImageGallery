using Microsoft.AspNetCore.Builder;

namespace ImageGallery.Api.Extentions.Middlewares
{
    public static class MiddlewareExtentions
    {
        public static IApplicationBuilder UseAppExceptions(this IApplicationBuilder app) =>
           app.UseMiddleware<ExceptionMiddleware>();
    }
}
