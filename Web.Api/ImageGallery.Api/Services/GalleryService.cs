using ImageGallery.Api.Factories;
using ImageGallery.Api.Helpers;
using ImageGallery.Contracts.Common;
using ImageGallery.Contracts.Common.Pagination;
using ImageGallery.Contracts.Data;
using ImageGallery.Contracts.Exceptions;
using ImageGallery.Contracts.Models;
using ImageGallery.Contracts.Services;
using ImageGallery.Specifications.GallerySpec;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Api.Services
{
    public sealed class GalleryService
        : IGalleryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GalleryService> _logger;
        private readonly IAsyncRepository<Gallery> _galleryRepository;

        public GalleryService(IUnitOfWork unitOfWork,
            ILogger<GalleryService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _galleryRepository = _unitOfWork.GetRepository<Gallery>();
        }

        public async Task<OperationResult<Gallery>> GetGalleryByIdAsync(int id)
        {
            try
            {
                var gallery = await _galleryRepository
                    .GetBySpecAsync(new GetGalleryByIdSpec(id));

                if (gallery is not null)
                    return OperationResult<Gallery>.Success(gallery);
                else
                    return OperationResult<Gallery>.Failed();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);

                return OperationResult<Gallery>.Failed();
            }
        }

        public async Task<OperationResult<IEnumerable<Gallery>>> GetGalleriesByUserIdAsync(string userId,
            PaginationFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException($"'{nameof(userId)}' cannot be null or empty", nameof(userId));
                if (filter is null)
                    throw new ArgumentNullException(nameof(filter));

                var (skip, take) = PaginationHelper.GetParameters(filter.PageNumber, filter.PageSize);

                var galleries = await _galleryRepository
                    .ListAsync(new GetGalleriesByUserIdSpec(userId, skip, take));

                if (galleries is not null)
                    return OperationResult<IEnumerable<Gallery>>.Success(galleries);
                else
                    return OperationResult<IEnumerable<Gallery>>.Failed();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);

                return OperationResult<IEnumerable<Gallery>>.Failed();
            }
        }

        public async Task<OperationResult<Gallery>> CreateGalleryAsync(AppUser user, Gallery newGallery)
        {
            try
            {
                if (user is null)
                    throw new ArgumentNullException(nameof(user));
                if (newGallery is null)
                    throw new ArgumentNullException(nameof(newGallery));

                newGallery.AppUserId = user.Id;

                _galleryRepository.Insert(newGallery);

                await _unitOfWork.SaveAsync();

                return OperationResult<Gallery>.Success(newGallery);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);

                return OperationResult<Gallery>.Failed(ex.Message);
            }
        }

        public async Task<OperationResult<Gallery>> UpdateGalleryAsync(int id, Gallery updatedGallery)
        {
            try
            {
                if (updatedGallery is null)
                    throw new ArgumentNullException(nameof(updatedGallery));

                var existingGallery = await _galleryRepository
                    .GetBySpecAsync(new GetGalleryByIdSpec(id));

                if (existingGallery is null)
                    throw new EntityNotFoundException(typeof(Gallery));

                UpdateGallery(existingGallery, updatedGallery);

                _galleryRepository.Update(existingGallery);

                await _unitOfWork.SaveAsync();

                return OperationResult<Gallery>.Success(existingGallery);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);

                return OperationResult<Gallery>.Failed(ex.Message);
            }
        }

        public async Task<OperationResult> DeleteGalleryAsync(int id, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException($"'{nameof(userId)}' cannot be null or empty", nameof(userId));

                var gallery = await _galleryRepository.ListAsync(new GetGalleryByIdSpec(id));

                _galleryRepository.Delete(gallery);

                await _unitOfWork.SaveAsync();


                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);

                return OperationResult.Failed();
            }
        }

        public async Task<OperationResult> DeleteGalleriesAsync(IEnumerable<int> ids, string userId)
        {
            if (ids is null || !ids.Any())
            {
                _logger.LogWarning("There is no galleries for deleting.");

                return OperationResult.Failed();
            }

            try
            {
                var galleries = await _galleryRepository.ListAsync(new GetGalleriesByIdsSpec(ids));

                var isUserManagesGalleries = galleries.All(g => g.AppUserId == userId);

                if (!isUserManagesGalleries)
                    return OperationResult.Failed();

                _galleryRepository.Delete(galleries);

                await _unitOfWork.SaveAsync();

                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);

                return OperationResult.Failed();
            }
        }

        public async Task<bool> CheckAccessGalleriesAsync(IEnumerable<int> ids, string userId)
        {
            if (ids is null)
                throw new ArgumentNullException(nameof(ids));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"'{nameof(userId)}' cannot be null or empty", nameof(userId));

            var galleries = await _galleryRepository.ListAsync(new GetGalleriesByIdsSpec(ids));

            return galleries.All(g => g.AppUserId == userId);
        }

        private static void UpdateGallery(Gallery existingGallery, Gallery updatedGallery) =>
            existingGallery.Name = updatedGallery.Name;
    }
}
