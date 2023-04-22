namespace Cloudy.CMS.UI.FormSupport
{
    public interface IEntityPathNavigator
    {
        void Navigate(ref object entity, ref string[] path);
    }
}