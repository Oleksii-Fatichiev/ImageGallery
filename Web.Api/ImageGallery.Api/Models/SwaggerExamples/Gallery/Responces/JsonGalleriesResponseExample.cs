using ImageGallery.Api.Models.Json;
using ImageGallery.Api.Models.Json.Gallery;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace ImageGallery.Api.Models.SwaggerExamples.Responces
{
    public sealed class JsonGalleriesResponseExample
        : IExamplesProvider<JsonPagedResponse<JsonGalleryResponse>>
    {
        public JsonPagedResponse<JsonGalleryResponse> GetExamples()
        {
            return new JsonPagedResponse<JsonGalleryResponse>
            {
                Data = new List<JsonGalleryResponse>
                {
                    new JsonGalleryResponse
                    {
                        Id = 100,
                        Name = "Art"
                    },
                    new JsonGalleryResponse
                    {
                        Id = 101,
                        Name = "Science"
                    }
                },
                PageNumber = 1,
                PageSize = 12,
                NextPage = null,
                PreviousPage = null
            };
        }
    }
}
