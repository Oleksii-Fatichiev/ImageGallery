using ImageGallery.Contracts.Common;
using ImageGallery.Contracts.Common.Pagination;
using ImageGallery.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageGallery.Contracts.Services
{
    public interface IPictureService
    {
        Task<OperationResult<IEnumerable<Picture>>> GetPicturesByGalleryIdAsync(int galleryId, PaginationFilter filter);

        Task<OperationResult> CreatePicturesAsync(IEnumerable<IFormFile> files, int galleryId);

        Task<OperationResult> DeletePicturesAsync(IEnumerable<int> ids, string userId);

        Task<bool> CheckAccessPicturesAsync(IEnumerable<int> ids, string id);
    }
}
