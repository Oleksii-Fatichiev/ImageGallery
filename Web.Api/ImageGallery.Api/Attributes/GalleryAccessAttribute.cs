using ImageGallery.Api.Extentions;
using ImageGallery.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ImageGallery.Api.Attributes
{
    public sealed class GalleryAccessAttribute
        : ActionFilterAttribute
    {
        public string GalleryIdParameter { get; }

        public GalleryAccessAttribute(string galleryIdparameter = "id") =>
            GalleryIdParameter = galleryIdparameter;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var scope = context.HttpContext.RequestServices;
            var logger = scope.GetService<ILogger<GalleryAccessAttribute>>();
            var accessCheckService = (IGalleryAccessCheckService)scope.GetService(typeof(IGalleryAccessCheckService));

            var id = context.GetRequestValue(GalleryIdParameter);

            if (!int.TryParse(id, out var requestedGalleryId))
            {
                logger.LogWarning(
                     $"No id parameter (gallery id) specified in url (controller = {context.RouteData.Values["controller"]}," +
                     $" action = {context.RouteData.Values["action"]}).");

                context.Result = new BadRequestObjectResult($"Missing {GalleryIdParameter} parameter.");

                return;
            }

            var result = await accessCheckService.HasAccess(requestedGalleryId, context.HttpContext.User);
            if (result.Succeeded)
            {
                await base.OnActionExecutionAsync(context, next);

                return;
            }

            var resultMessage = string.Join(Environment.NewLine, result.Errors);

            var logMessage =
                $"{resultMessage} (controller = {context.RouteData.Values["controller"]}," +
                $" action = {context.RouteData.Values["action"]}, gallery id = {requestedGalleryId}).";

            logger.LogWarning(logMessage);

            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Content = resultMessage
            };
        }
    }
}
