using ImageGallery.Api.Models.Json;
using ImageGallery.Contracts.Common.Pagination;
using ImageGallery.Contracts.Services;
using System.Collections.Generic;
using System.Linq;

namespace ImageGallery.Api.Helpers
{
    public sealed class PaginationHelper
    {
        public static JsonPagedResponse<T> CreatePaginationResponse<T>(IUriService uriService,
            PaginationFilter filter, IEnumerable<T> response)
        {
            var nextPage = filter.PageNumber >= 1 && response.Count() == filter.PageSize
                ? GetUri(uriService, filter.PageNumber + 1, filter.PageSize)
                : null;

            var previousPage = filter.PageNumber - 1 >= 1
                ? GetUri(uriService, filter.PageNumber - 1, filter.PageSize)
                : null;

            return new JsonPagedResponse<T>
            {
                Data = response,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                NextPage = nextPage,
                PreviousPage = previousPage
            };
        }

        public static (int skip, int take) GetParameters(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;

            return (skip, pageSize);
        }

        private static string GetUri(IUriService uriService,
            int pageNumber,
            int pageSize)
        {
            return uriService.GetGalleriesUri(new PaginationFilter(pageNumber, pageSize)).ToString();
        }
    }
}
