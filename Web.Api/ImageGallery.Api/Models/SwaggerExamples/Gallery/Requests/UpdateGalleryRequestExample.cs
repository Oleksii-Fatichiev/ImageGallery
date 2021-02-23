using ImageGallery.Api.Models.Json.Gallery;
using Swashbuckle.AspNetCore.Filters;

namespace ImageGallery.Api.Models.SwaggerExamples.Gallery.Request
{
    public sealed class UpdateGalleryRequestExample
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
