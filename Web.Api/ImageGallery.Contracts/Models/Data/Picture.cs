namespace ImageGallery.Contracts.Models
{
    public class Picture
        : Entity
    {
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }

        public int GalleryId { get; set; }
        public virtual Gallery Gallery { get; set; }
    }
}
