using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poetry.ComponentSupport;

namespace Poetry.UI.StyleSupport
{
    public class StyleProvider : IStyleProvider
    {
        IDictionary<string, IEnumerable<StyleDescriptor>> Styles { get; }

        public StyleProvider(IComponentProvider componentProvider, IStyleCreator styleCreator)
        {
            Styles = componentProvider.GetAll().ToDictionary(c => c.Id, c => (IEnumerable<StyleDescriptor>)styleCreator.Create(c).ToList().AsReadOnly());
        }

        public IEnumerable<StyleDescriptor> GetAllFor(ComponentDescriptor component)
        {
            return Styles[component.Id];
        }
    }
}
