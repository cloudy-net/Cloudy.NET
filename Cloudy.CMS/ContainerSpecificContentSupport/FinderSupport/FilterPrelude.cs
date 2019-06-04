namespace Cloudy.CMS.ContainerSpecificContentSupport.FinderSupport
{
    public class FilterPrelude<T1, T2> where T1 : class
    {
        QueryBuilder<T1> QueryBuilder { get; }
        string Property { get; }

        public FilterPrelude(QueryBuilder<T1> queryBuilder, string property)
        {
            QueryBuilder = queryBuilder;
            Property = property;
        }

        public QueryBuilder<T1> EqualTo(string value)
        {
            QueryBuilder.AddFilter(new EqualToFilter(Property, value));

            return QueryBuilder;
        }
    }
}