namespace Cloudy.CMS.ContainerSpecificContentSupport.FinderSupport
{
    public class EqualToFilter : IFilter
    {
        public string Property { get; }
        public string Value { get; }

        public EqualToFilter(string property, string value)
        {
            Property = property;
            Value = value;
        }
    }
}