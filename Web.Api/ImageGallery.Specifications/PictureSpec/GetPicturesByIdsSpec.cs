using Ardalis.Specification;
using ImageGallery.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageGallery.Specifications.PictureSpec
{
    public sealed class GetPicturesByIdsSpec
        : Specification<Picture>
    {
        public GetPicturesByIdsSpec(IEnumerable<int> ids)
        {
            Query.Where(p => ids.Contains(p.Id));
        }
    }
}
