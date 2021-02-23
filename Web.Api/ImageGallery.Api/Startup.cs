using AutoMapper;
using FluentValidation.AspNetCore;
using ImageGallery.Api.Common.ApiVersioning;
using ImageGallery.Api.Extentions.Middlewares;
using ImageGallery.Api.Extentions.Swagger;
using ImageGallery.Api.Services;
using ImageGallery.Contracts.Models;
using ImageGallery.Contracts.Models.Options;
using ImageGallery.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Net.Mime;
using System.Text;

namespace ImageGallery.Api
{
    public class Startup
    {
        private const string JWT_SCHEME = JwtBearerDefaults.AuthenticationScheme;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            // Add app dependencies
            services.AddDependencies();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>())
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var result = new BadRequestObjectResult(context.ModelState);

                        result.ContentTypes.Add(MediaTypeNames.Application.Json);

                        return result;
                    };
                });

            // Configure DB service
            services.AddDbContext<ImageGalleryDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Configure Identity service
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ImageGalleryDbContext>()
            .AddDefaultTokenProviders();

            // Configure Auth service
            var jwtSection = Configuration.GetSection(JwtOptions.JWT_OPTIONS);
            services.Configure<JwtOptions>(options => jwtSection.Bind(options));
            var jwtSettings = jwtSection.Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JWT_SCHEME;
                options.DefaultChallengeScheme = JWT_SCHEME;
            })
            .AddJwtBearer(JWT_SCHEME, options =>
            {
                //options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ValidateLifetime = jwtSettings.ValidateLifetime,
                    ClockSkew = TimeSpan.FromMinutes(jwtSettings.ClockSkew)
                };
            });

            // Configure Swagger service
            services.ConfigureSwagger();

            // Add Api Versioning
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;

                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

                options.ApiVersionReader = ApiVersionReader.Combine(
                    //new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("api-version"));

                options.ErrorResponses = new ApiVersioningErrorResponseProvider();
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            // Configure Options
            services.AddOptions<JwtOptions>()
                .Bind(Configuration.GetSection(JwtOptions.JWT_OPTIONS));

            services.AddCors(options =>
                options.AddDefaultPolicy(p =>
                    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
        }

        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseAppExceptions();

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            });

            app.UseStaticFiles();
            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
