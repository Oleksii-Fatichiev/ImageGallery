using System;

namespace ImageGallery.Api.Models.Json.Auth
{
    public sealed class JsonJwtToken
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
