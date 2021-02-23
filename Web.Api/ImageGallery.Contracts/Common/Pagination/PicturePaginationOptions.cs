namespace ImageGallery.Contracts.Common.Pagination
{
    public sealed class PicturePaginationOptions
        : IPaginationOptions
    {
        public int PageNumber { get; } = 1;
        public int PageSize { get; } = 8;
    }
}
