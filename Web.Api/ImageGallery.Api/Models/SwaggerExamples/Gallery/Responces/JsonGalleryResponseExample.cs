using ImageGallery.Api.Models.Json.Gallery;
using Swashbuckle.AspNetCore.Filters;

namespace ImageGallery.Api.Models.SwaggerExamples.Responces
{
    public sealed class JsonGalleryResponseExample
        : IExamplesProvider<JsonGalleryResponse>
    {
        public JsonGalleryResponse GetExamples()
        {
            return new JsonGalleryResponse
            {
                Id = 100,
                Name = "Art"
            };
        }
    }
}
