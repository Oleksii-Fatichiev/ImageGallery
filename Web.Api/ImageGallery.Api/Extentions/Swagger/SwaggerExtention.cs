using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

namespace ImageGallery.Api.Extentions.Swagger
{
    public static class SwaggerExtention
    {

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.OperationFilter<SwaggerDefaultValues>();

                swagger.ExampleFilters();
            });

            // To add request/responce examples to Swagger UI.
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }
    }
}
