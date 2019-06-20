namespace Poetry.UI.DataTableSupport.BackendSupport
{
    public class Query
    {
        public int Page { get; }
        public string SortBy { get; }
        public SortDirection? SortDirection { get; }

        public Query(int page, string sortBy, SortDirection? sortDirection)
        {
            Page = page;
            SortBy = sortBy;
            SortDirection = sortDirection;
        }
    }
}