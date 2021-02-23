using ImageGallery.Contracts.Common.Pagination;
using ImageGallery.Contracts.Services;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace ImageGallery.Api.Services
{
    public class UriService
        : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetGalleriesUri(PaginationFilter filter)
        {
            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());

            return new Uri(modifiedUri);
        }
    }
}
