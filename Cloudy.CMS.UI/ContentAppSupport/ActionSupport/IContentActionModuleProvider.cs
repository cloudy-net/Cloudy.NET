using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ActionSupport
{
    public interface IContentActionModuleProvider
    {
        IEnumerable<string> GetModulesFor(string contentTypeId);
    }
}
