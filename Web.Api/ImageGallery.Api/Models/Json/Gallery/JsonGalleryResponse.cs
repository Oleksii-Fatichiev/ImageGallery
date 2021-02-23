namespace ImageGallery.Api.Models.Json.Gallery
{
    public sealed class JsonGalleryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public ICollection<JsonPicture> Pictures { get; set; }

        //public JsonGalleryResponse() => Pictures = new HashSet<JsonPicture>();
    }
}
