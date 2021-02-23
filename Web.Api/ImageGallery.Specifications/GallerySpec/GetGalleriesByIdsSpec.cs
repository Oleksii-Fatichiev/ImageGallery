using Ardalis.Specification;
using ImageGallery.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageGallery.Specifications.GallerySpec
{
    public sealed class GetGalleriesByIdsSpec
           : Specification<Gallery>
    {
        public GetGalleriesByIdsSpec(IEnumerable<int> ids) =>
            Query.Where(g => ids.Contains(g.Id));
    }
}
