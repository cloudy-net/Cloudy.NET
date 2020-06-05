using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    [Area("Cloudy.CMS")]
    [Route("SelectControl")]
    public class SelectControlApiController
    {
        IDictionary<string, IItemProvider> OptionProviders { get; }

        public SelectControlApiController(IComposableProvider composableProvider)
        {
            OptionProviders = composableProvider.GetAll<IItemProvider>().ToDictionary(p => p.GetType().GetCustomAttribute<ItemProviderAttribute>().Id, p => p);
        }

        [Route("GetItem")]
        public async Task<Item> GetItem(string provider, string type, string value)
        {
            return await OptionProviders[provider].Get(type, value).ConfigureAwait(false);
        }

        [Route("GetItems")]
        public async Task<IEnumerable<Item>> GetItems(string provider, string type)
        {
            return await OptionProviders[provider].GetAll(type).ConfigureAwait(false);
        }
    }
}
