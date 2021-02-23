using AutoMapper;
using ImageGallery.Api.Models.Json;
using ImageGallery.Api.Models.Json.Gallery;
using ImageGallery.Contracts.Common.Pagination;
using ImageGallery.Contracts.Models;

namespace ImageGallery.Api.Mapping
{
    public sealed class ApiProfile
        : Profile
    {
        public ApiProfile()
        {
            CreateMap<JsonGalleryResponse, Gallery>()
                .ReverseMap();

            CreateMap<JsonGalleryRequest, Gallery>();

            CreateMap<JsonPicture, Picture>()
                .ReverseMap();

            CreateMap<JsonPaginationQuery, PaginationFilter>();
        }
    }
}
