using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Cloudy.CMS.UI.FormSupport.Controls.DropdownControlSupport
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class DropdownControlController
    {
        IDictionary<string, IOptionProvider> OptionProviders { get; }

        public DropdownControlController(IComposableProvider composableProvider)
        {
            OptionProviders = composableProvider.GetAll<IOptionProvider>().ToDictionary(p => p.GetType().GetCustomAttribute<OptionProviderAttribute>().Id, p => p);
        }

        public IEnumerable<Option> GetOptions(string provider)
        {
            return OptionProviders[provider].GetAll();
        }
    }
}
