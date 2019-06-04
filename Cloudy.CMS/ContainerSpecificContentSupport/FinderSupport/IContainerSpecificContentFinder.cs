namespace Cloudy.CMS.ContainerSpecificContentSupport.FinderSupport
{
    public interface IContainerSpecificContentFinder
    {
        QueryBuilder<T> Find<T>(string container) where T : class;
    }
}