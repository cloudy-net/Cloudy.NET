using Cloudy.CMS.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.StyleSupport
{
    public interface IStyleProvider
    {
        IEnumerable<StyleDescriptor> GetAll();
    }
}
