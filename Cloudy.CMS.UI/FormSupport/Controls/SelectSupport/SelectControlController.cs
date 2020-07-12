using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class SelectControlController : Controller
    {
        IDictionary<string, IItemProvider> OptionProviders { get; }

        public SelectControlController(IComposableProvider composableProvider)
        {
            OptionProviders = composableProvider.GetAll<IItemProvider>().ToDictionary(p => p.GetType().GetCustomAttribute<ItemProviderAttribute>().Id, p => p);
        }

        public async Task<ActionResult> GetItem(string provider, string type, string value)
        {
            var result = await OptionProviders[provider].Get(type, value).ConfigureAwait(false);

            if(result == null)
            {
                return NotFound();
            }

            return Json(result);
        }

        public async Task<IEnumerable<Item>> GetItems(string provider, string type, string parent)
        {
            return await OptionProviders[provider].GetAll(type, new ItemQuery { Parent = parent }).ConfigureAwait(false);
        }
    }
}
