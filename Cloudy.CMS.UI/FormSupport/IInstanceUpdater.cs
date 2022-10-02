using Microsoft.AspNetCore.Http;

namespace Cloudy.CMS.UI.FormSupport
{
    public interface IInstanceUpdater
    {
        void Update(string contentType, object instance, IFormCollection form);
    }
}