namespace ImageGallery.Api.Models.Json
{
    public sealed class JsonPicture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }
        public int GalleryId { get; set; }
    }
}
