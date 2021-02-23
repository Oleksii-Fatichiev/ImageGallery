using System.Collections.Generic;

namespace ImageGallery.Api.Models.Json
{
    public sealed class JsonValidationError
    {
        public List<JsonValidationErrorModel> Errors { get; set; }
    }
}
