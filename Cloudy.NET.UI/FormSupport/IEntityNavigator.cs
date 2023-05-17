namespace Cloudy.NET.UI.FormSupport
{
    public interface IEntityNavigator
    {
        object Navigate(object entity, string[] path, IListTracker listTracker);
    }
}