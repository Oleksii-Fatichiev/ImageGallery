namespace ImageGallery.Api.Models.Json.Picture
{
    public sealed class JsonPictureResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
}
}
