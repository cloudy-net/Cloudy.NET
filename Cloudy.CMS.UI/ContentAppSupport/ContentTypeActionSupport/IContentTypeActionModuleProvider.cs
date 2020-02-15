using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport
{
    public interface IContentTypeActionModuleProvider
    {
        IEnumerable<string> GetContentTypeActionModulesFor(string contentTypeId);
    }
}
