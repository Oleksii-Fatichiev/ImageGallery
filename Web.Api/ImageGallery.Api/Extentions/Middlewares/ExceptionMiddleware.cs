using ImageGallery.Api.Models.Json;
using ImageGallery.Contracts.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ImageGallery.Api.Extentions.Middlewares
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            if (next is null)
                throw new ArgumentNullException(nameof(next));

            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong: {ex.Message}");

                await HandleExeptionAsync(context);
            }
        }

        private static Task HandleExeptionAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = Constants.ContentTypes.JSON;

            return context.Response.WriteAsync(new JsonErrorResponse
            {
                Message = HttpStatusCode.InternalServerError.ToString()
            }
            .ToString());
        }
    }
}
