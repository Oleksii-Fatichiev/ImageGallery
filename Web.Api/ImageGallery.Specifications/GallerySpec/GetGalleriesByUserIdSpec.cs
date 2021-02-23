using Ardalis.Specification;
using ImageGallery.Contracts.Models;

namespace ImageGallery.Specifications.GallerySpec
{
    public sealed class GetGalleriesByUserIdSpec
        : Specification<Gallery>
    {
        public GetGalleriesByUserIdSpec(string userId, int skip, int take) =>
            Query.Where(g => g.AppUserId == userId)
            .Skip(skip)
            .Take(take);
    }
}

