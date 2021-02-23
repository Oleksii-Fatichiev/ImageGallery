using ImageGallery.Api.Models.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Api.Filters
{
    public sealed class ValidationFilter
        : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .ToDictionary(ms => ms.Key, ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                var errorResponse = new JsonValidationError();
                foreach (var error in errors)
                {
                    foreach (var suberror in error.Value)
                    {
                        errorResponse.Errors.Add(new JsonValidationErrorModel
                        {
                            FieldName = error.Key,
                            Message = suberror
                        });
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);

                return;
            }

            await next();
        }
    }
}
