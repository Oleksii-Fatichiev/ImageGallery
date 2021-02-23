using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Net;

namespace ImageGallery.Api.Common.ApiVersioning
{
    public sealed class ApiVersioningErrorResponseProvider
        : DefaultErrorResponseProvider
    {
        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            var errorResponse = new
            {
                ResponseMessages = "Something went wrong while selecting the api version",
            };

            return new ObjectResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }
}
