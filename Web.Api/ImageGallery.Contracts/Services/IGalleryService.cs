using ImageGallery.Contracts.Common;
using ImageGallery.Contracts.Common.Pagination;
using ImageGallery.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageGallery.Contracts.Services
{
    public interface IGalleryService
    {
        Task<OperationResult<Gallery>> GetGalleryByIdAsync(int id);

        Task<OperationResult<IEnumerable<Gallery>>> GetGalleriesByUserIdAsync(string id,
            PaginationFilter filter);

        Task<OperationResult<Gallery>> CreateGalleryAsync(AppUser user, Gallery newGallery);

        Task<OperationResult<Gallery>> UpdateGalleryAsync(int id, Gallery updatedGallery);

        Task<OperationResult> DeleteGalleryAsync(int id, string userId);

        Task<OperationResult> DeleteGalleriesAsync(IEnumerable<int> ids, string userId);

        Task<bool> CheckAccessGalleriesAsync(IEnumerable<int> ids, string userId);
    }
}
