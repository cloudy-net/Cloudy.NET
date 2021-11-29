using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.SingletonSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Cloudy.CMS.ContentSupport.Serialization;
using System.Linq;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class SingletonController : Controller
    {
        ISingletonGetter SingletonGetter { get; }
        IContentJsonConverterProvider ContentJsonConverterProvider { get; }

        public SingletonController(ISingletonGetter singletonGetter, IContentJsonConverterProvider contentJsonConverterProvider)
        {
            SingletonGetter = singletonGetter;
            ContentJsonConverterProvider = contentJsonConverterProvider;
        }

        public async Task<ActionResult> Get(string id)
        {
            var options = new JsonSerializerOptions();
            ContentJsonConverterProvider.GetAll().ToList().ForEach(c => options.Converters.Add(c));

            return Json(await SingletonGetter.GetAsync(id).ConfigureAwait(false), options);
        }
    }
}
