using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;

namespace Cloudy.CMS.UI.StyleSupport
{
    public class StyleProvider : IStyleProvider
    {
        IEnumerable<StyleDescriptor> Styles { get; }

        public StyleProvider(IComposableProvider composableProvider)
        {
            Styles = composableProvider.GetAll<IStyleCreator>().SelectMany(c => c.Create()).ToList().AsReadOnly();
        }

        public IEnumerable<StyleDescriptor> GetAll()
        {
            return Styles;
        }
    }
}
