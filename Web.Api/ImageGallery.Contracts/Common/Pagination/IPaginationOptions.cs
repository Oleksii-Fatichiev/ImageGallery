namespace ImageGallery.Contracts.Common.Pagination
{
    public interface IPaginationOptions
    {
        int PageNumber { get; }
        int PageSize { get; }
    }
}