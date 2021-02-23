using Newtonsoft.Json;

namespace ImageGallery.Api.Models.Json
{
    public sealed class JsonErrorResponse
    {
        public string Message { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
