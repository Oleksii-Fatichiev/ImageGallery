using ImageGallery.Contracts.Common;
using ImageGallery.Contracts.Data;
using ImageGallery.Contracts.Models;
using ImageGallery.Contracts.Services;
using ImageGallery.Specifications.GallerySpec;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ImageGallery.Api.Services
{
    public sealed class GalleryAccessCheckService
        : IGalleryAccessCheckService
    {
        private const string ACCESS_DENIED = "Access denied.";

        private readonly UserManager<AppUser> _userManager;
        private readonly IAsyncRepository<Gallery> _galleryRepository;

        public GalleryAccessCheckService(UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

            if (unitOfWork is null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _galleryRepository = unitOfWork.GetRepository<Gallery>();
        }

        public async Task<OperationResult> HasAccess(int galleryId, IPrincipal user)
        {
            if (user?.Identity?.Name is null)
                return OperationResult.Failed(ACCESS_DENIED);

            var appUser = await _userManager.FindByNameAsync(user.Identity.Name);

            if (appUser is null)
                return OperationResult.Failed(ACCESS_DENIED);

            var gallery = await _galleryRepository.GetBySpecAsync(new GetGalleryByIdSpec(galleryId));

            return gallery?.AppUserId == appUser.Id
                ? OperationResult.Success
                : OperationResult.Failed(ACCESS_DENIED);
        }
    }
}
