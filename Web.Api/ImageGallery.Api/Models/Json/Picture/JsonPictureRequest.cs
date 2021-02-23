namespace ImageGallery.Api.Models.Json.Picture
{
    public sealed class JsonPictureRequest
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
