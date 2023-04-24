namespace Cloudy.CMS.UI.FormSupport
{
    public interface IEntityNavigator
    {
        object Navigate(object entity, string[] path);
    }
}