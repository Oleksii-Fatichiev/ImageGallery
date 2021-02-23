using ImageGallery.Api.Helpers;
using ImageGallery.Contracts.Common;
using ImageGallery.Contracts.Common.Pagination;
using ImageGallery.Contracts.Constants;
using ImageGallery.Contracts.Data;
using ImageGallery.Contracts.Models;
using ImageGallery.Contracts.Services;
using ImageGallery.Specifications.PictureSpec;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Api.Services
{
    public sealed class PictureService
        : IPictureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PictureService> _logger;
        private readonly IAsyncRepository<Picture> _pictureRepository;

        public PictureService(IUnitOfWork unitOfWork,
            ILogger<PictureService> logger)
        {
            _unitOfWork = unitOfWork
                ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            _pictureRepository = unitOfWork.GetRepository<Picture>();
        }

        public async Task<OperationResult<IEnumerable<Picture>>> GetPicturesByGalleryIdAsync(int galleryId,
            PaginationFilter filter)
        {
            try
            {
                if (filter is null)
                    throw new ArgumentNullException(nameof(filter));

                var (skip, take) = PaginationHelper.GetParameters(filter.PageNumber, filter.PageSize);

                var pictures = await _pictureRepository
                    .ListAsync(new GetPicturesByGalleryIdSpec(galleryId, skip, take));

                return OperationResult<IEnumerable<Picture>>.Success(pictures);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);

                return OperationResult<IEnumerable<Picture>>.Failed();
            }
        }

        public async Task<OperationResult> CreatePicturesAsync(IEnumerable<IFormFile> files, int galleryId)
        {
            try
            {
                if (files is null)
                    throw new ArgumentNullException(nameof(files));

                var pictures = CreatePictures(files, galleryId);

                _pictureRepository.Insert(pictures);

                await _unitOfWork.SaveAsync();

                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);

                return OperationResult.Failed();
            }
        }

        public async Task<OperationResult> DeletePicturesAsync(IEnumerable<int> ids, string userId)
        {
            try
            {
                if (ids is null)
                    throw new ArgumentNullException(nameof(ids));

                var deletingPictures = await _pictureRepository
                    .ListAsync(new GetPicturesByIdsWithGalleriesSpec(ids));

                _pictureRepository.Delete(deletingPictures);

                await _unitOfWork.SaveAsync();

                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);

                return OperationResult.Failed();
            }
        }

        public async Task<bool> CheckAccessPicturesAsync(IEnumerable<int> ids, string userId)
        {
            return await _pictureRepository.AnyAsync(p => ids.Contains(p.Id) && p.Gallery.AppUserId != userId);
        }

        private static IEnumerable<Picture> CreatePictures(IEnumerable<IFormFile> files, int galleryId)
        {
            var pictures = new List<Picture>();

            foreach (var file in files)
            {
                if (!(file.Length > 0))
                    break;

                var fileExtention = Path.GetExtension(file.FileName);

                if (!Constants.ImageFormats.Contains(fileExtention.ToLower()))
                    break;

                var fileName = $"{new Guid()}{DateTime.UtcNow}{fileExtention}";

                var picture = new Picture
                {
                    Name = fileName,
                    MimeType = fileExtention,
                    GalleryId = galleryId
                };

                using (var ms = new MemoryStream())
                {
                    file.CopyToAsync(ms);
                    picture.Data = ms.ToArray();
                };

                pictures.Add(picture);
            }

            return pictures;
        }
    }
}
