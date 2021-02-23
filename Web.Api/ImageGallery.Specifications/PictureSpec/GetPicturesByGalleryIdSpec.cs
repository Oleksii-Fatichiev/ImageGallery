using Ardalis.Specification;
using ImageGallery.Contracts.Models;

namespace ImageGallery.Specifications.PictureSpec
{
    public sealed class GetPicturesByGalleryIdSpec
        : Specification<Picture>
    {
        public GetPicturesByGalleryIdSpec(int id, int skip, int take) =>
            Query.Where(p => p.GalleryId == id)
            .Skip(skip)
            .Take(take);
    }
}
