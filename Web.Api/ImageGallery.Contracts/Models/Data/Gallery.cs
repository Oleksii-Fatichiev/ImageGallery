using System.Collections.Generic;

namespace ImageGallery.Contracts.Models
{
    public class Gallery
        : Entity
    {
        public string Name { get; set; }

        public string AppUserId { get; set; }
        public virtual AppUser User { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

        public Gallery() => Pictures = new HashSet<Picture>();
    }
}
