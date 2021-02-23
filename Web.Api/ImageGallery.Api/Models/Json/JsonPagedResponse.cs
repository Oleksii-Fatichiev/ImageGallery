using System.Collections.Generic;

namespace ImageGallery.Api.Models.Json
{
    public sealed class JsonPagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }

        public JsonPagedResponse()
        {
        }

        public JsonPagedResponse(IEnumerable<T> data)
        {
            Data = data;
        }
    }
}
