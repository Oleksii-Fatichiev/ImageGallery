using Ardalis.Specification;
using ImageGallery.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageGallery.Specifications.PictureSpec
{
    public sealed class GetPicturesByIdsWithGalleriesSpec
        : Specification<Picture>
    {
        public GetPicturesByIdsWithGalleriesSpec(IEnumerable<int> ids)
        {
            Query.Where(p => ids.Contains(p.Id));
        }
    }
}
