using System.Text.Json.Serialization;

namespace Postex.SharedKernel.Paginations
{

    public class PagedList<T>
    {
        [JsonPropertyName("page_index")]
        public int PageIndex { get; set; }

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }

        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("index_from")]
        public int IndexFrom { get; set; }

        public IList<T> Items { get; set; }

        [JsonPropertyName("has_prev_page")]
        public bool HasPreviousPage => PageIndex - IndexFrom > 0;

        [JsonPropertyName("has_next_page")]
        public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;


        public PagedList(IEnumerable<T> source, int pageNumber, int pageSize, int totalCount)
        {
            PageIndex = pageNumber;
            PageSize = pageSize;
            IndexFrom = pageNumber * pageSize;

            var itemList = source.ToList();
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            Items = itemList.Skip(IndexFrom).Take(PageSize).ToList();
        }

        public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            PageIndex = pageNumber;
            PageSize = pageSize;
            IndexFrom = pageNumber * pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Items = source.Skip(IndexFrom).Take(PageSize).ToList();
        }

        public PagedList()
        {
            Items = new T[0];
        }
    }

}