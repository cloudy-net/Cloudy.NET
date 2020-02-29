using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.Controls.DropdownControlSupport
{
    [Area("Cloudy.CMS")]
    [Route("DropdownControl")]
    public class DropdownControlApiController
    {
        IDictionary<string, IOptionProvider> OptionProviders { get; }

        public DropdownControlApiController(IComposableProvider composableProvider)
        {
            OptionProviders = composableProvider.GetAll<IOptionProvider>().ToDictionary(p => p.GetType().GetCustomAttribute<OptionProviderAttribute>().Id, p => p);
        }

        [Route("GetOptions")]
        public IEnumerable<Option> GetOptions(string provider)
        {
            return OptionProviders[provider].GetAll();
        }
    }
}
