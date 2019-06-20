using Poetry.ComposableSupport;
using Poetry.UI.ApiSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Poetry.UI.FormSupport.Controls.DropdownControlSupport
{
    [Api("DropdownControl")]
    public class DropdownControlApi
    {
        IDictionary<string, IOptionProvider> OptionProviders { get; }

        public DropdownControlApi(IComposableProvider composableProvider)
        {
            OptionProviders = composableProvider.GetAll<IOptionProvider>().ToDictionary(p => p.GetType().GetCustomAttribute<OptionProviderAttribute>().Id, p => p);
        }

        [Endpoint("GetOptions")]
        public IEnumerable<Option> GetOptions(string provider)
        {
            return OptionProviders[provider].GetAll();
        }
    }
}
