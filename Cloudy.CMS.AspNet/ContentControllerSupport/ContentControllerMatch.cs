using Cloudy.CMS.ContentControllerSupport;

namespace Cloudy.CMS.AspNet.ContentControllerSupport
{
    public class ContentControllerMatch : IContentControllerMatch
    {
        public string ControllerName { get; }
        public string ControllerAction { get; }
        public string ParameterName { get; }

        public ContentControllerMatch(string controllerName, string controllerAction, string parameterName)
        {
            ControllerName = controllerName;
            ControllerAction = controllerAction;
            ParameterName = parameterName;
        }
    }
}