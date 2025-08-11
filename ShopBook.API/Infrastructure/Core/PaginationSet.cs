namespace ShopBook.API.Infrastructure.Core
{
    public class PaginationSet<T>
    {
        public int Page { set; get; }

        public int Count
        {
            get
            {
                return Data != null ? Data.Count() : 0;
            }
        }

        public int TotalPages { set; get; }
        public int TotalCount { set; get; }
        public int MaxPage { set; get; }
        public IEnumerable<T>? Data { set; get; }
    }
}
