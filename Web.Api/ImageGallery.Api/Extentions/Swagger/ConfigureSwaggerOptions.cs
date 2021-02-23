using ImageGallery.Contracts.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace ImageGallery.Api.Extentions.Swagger
{
    public sealed class ConfigureSwaggerOptions
        : IConfigureOptions<SwaggerGenOptions>
    {
        private const string JWT_SCHEME = JwtBearerDefaults.AuthenticationScheme;
        private const string DESCRIPTION = " This API version has been deprecated.";

        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));

            // To create an XML file for API documentation.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath, true);

            // To Enable authorization using Swagger (JWT)    
            options.AddSecurityDefinition(JWT_SCHEME, new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = JWT_SCHEME,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JWT_SCHEME
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            options.ExampleFilters();
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = Constants.SwaggerOptions.TITLE,
                Version = description.ApiVersion.ToString(),
            };

            if (description.IsDeprecated)
                info.Description += DESCRIPTION;

            return info;
        }
    }
}