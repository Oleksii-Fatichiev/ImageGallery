using ImageGallery.Api.Models.Json.Gallery;
using Swashbuckle.AspNetCore.Filters;

namespace ImageGallery.Api.Models.SwaggerExamples.Gallery.Requests
{
    public sealed class CreateGalleryRequestExample
        : IExamplesProvider<JsonGalleryRequest>
    {
        public JsonGalleryRequest GetExamples()
        {
            return new JsonGalleryRequest
            {
                Name = "Art"
            };
        }
    }
}
