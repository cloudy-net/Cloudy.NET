namespace Cloudy.CMS.ContentControllerSupport
{
    public interface IContentControllerMatch
    {
        string ControllerName { get; }
        string ControllerAction { get; }
    }
}