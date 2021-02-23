using Ardalis.Specification;
using ImageGallery.Contracts.Models;

namespace ImageGallery.Specifications.GallerySpec
{
    public sealed class GetGalleryByIdSpec
           : Specification<Gallery>
    {
        public GetGalleryByIdSpec(int id)
        {
            Query.Where(g => g.Id == id);
        }
    }
}
