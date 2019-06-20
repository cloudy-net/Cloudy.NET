using Cloudy.CMS.ContentControllerSupport;

namespace Cloudy.CMS.AspNetCore.ContentControllerSupport
{
    public class ContentControllerMatch : IContentControllerMatch
    {
        public string ControllerName { get; }
        public string ControllerAction { get; }

        public ContentControllerMatch(string controllerName, string controllerAction)
        {
            ControllerName = controllerName;
            ControllerAction = controllerAction;
        }
    }
}