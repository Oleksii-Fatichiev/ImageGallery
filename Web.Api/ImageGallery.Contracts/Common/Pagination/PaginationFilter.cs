namespace ImageGallery.Contracts.Common.Pagination
{
    public sealed class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationFilter()
        {
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PaginationFilter(IPaginationOptions options)
        {
            PageNumber = options.PageNumber;
            PageSize = options.PageSize > 100 ? 100 : options.PageSize;
        }
    }
}
