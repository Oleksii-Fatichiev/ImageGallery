using ImageGallery.Contracts.Common;
using ImageGallery.Contracts.Common.Pagination;
using System;

namespace ImageGallery.Contracts.Services
{
    public interface IUriService
    {
        //string GetGalleryUri(int id);

        Uri GetGalleriesUri(PaginationFilter filter = null);
    }
}
