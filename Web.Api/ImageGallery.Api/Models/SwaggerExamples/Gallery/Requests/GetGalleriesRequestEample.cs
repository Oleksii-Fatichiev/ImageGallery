using ImageGallery.Api.Models.Json;
using Swashbuckle.AspNetCore.Filters;

namespace ImageGallery.Api.Models.SwaggerExamples.Gallery.Requests
{
    public sealed class GetGalleriesRequestEample
        : IExamplesProvider<JsonPaginationQuery>
    {
        public JsonPaginationQuery GetExamples()
        {
            return new JsonPaginationQuery
            {
                PageNumber = 2,
                PageSize = 12
            };
        }
    }
}
