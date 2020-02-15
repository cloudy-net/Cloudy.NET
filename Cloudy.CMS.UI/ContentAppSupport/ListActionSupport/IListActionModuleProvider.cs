using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ListActionSupport
{
    public interface IListActionModuleProvider
    {
        IEnumerable<string> GetListActionModulesFor(string contentTypeId);
    }
}
