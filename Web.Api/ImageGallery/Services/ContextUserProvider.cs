using ImageGallery.Contracts.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace ImageGallery.Services
{
    public sealed class ContextUserProvider
        : IContextUserProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ContextUserProvider(IHttpContextAccessor contextAccessor) =>
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));

        public string GetCurrentUserId() =>
            _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
