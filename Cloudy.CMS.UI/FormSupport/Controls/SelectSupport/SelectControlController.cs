using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

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

        public async Task<ItemCreationResultMessage> CreateItem([FromBody] ItemCreationModel model)
        {
            var provider = OptionProviders[model.Provider];
            var type = provider.GetType();
            var @interface = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IItemCreator<>));

            if (@interface == null)
            {
                return null;
            }

            var item = JsonConvert.DeserializeObject(model.Item, @interface.GetGenericArguments()[0]);

            @interface.GetMethod(nameof(IItemCreator<object>.CreateAsync)).Invoke(provider, new object[] { item });

            return new ItemCreationResultMessage {  };
        }

        public string GetCreationForm(string provider)
        {
            var type = OptionProviders[provider].GetType();
            var model = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IItemCreator<>))?.GetGenericArguments()[0];

            if(model == null)
            {
                return null;
            }

            var attribute = model.GetCustomAttribute<FormAttribute>();

            if(attribute == null)
            {
                throw new Exception("Type parameter IItemCreator<T> must have the attribute [Form(...)]");
            }

            return attribute.Id;
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
