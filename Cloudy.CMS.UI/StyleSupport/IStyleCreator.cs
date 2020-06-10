using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.StyleSupport
{
    public interface IStyleCreator : IComposable
    {
        IEnumerable<StyleDescriptor> Create();
    }
}
