using ImageGallery.Api.Extentions.Swagger;
using ImageGallery.Contracts.Data;
using ImageGallery.Contracts.Services;
using ImageGallery.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ImageGallery.Api.Services
{
    public static class DIService
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddScoped<IUnitOfWork, ImageGalleryDbContext>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGalleryService, GalleryService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = $"{request.Scheme}://{request.Host.ToUriComponent()}/";

                return new UriService(absoluteUri);
            });

            services.AddScoped<IGalleryAccessCheckService, GalleryAccessCheckService>();
        }
    }
}
