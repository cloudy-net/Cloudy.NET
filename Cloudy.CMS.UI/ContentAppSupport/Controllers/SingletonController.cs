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

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class SingletonController : Controller
    {
        ISingletonGetter SingletonGetter { get; }

        public SingletonController(ISingletonGetter singletonGetter)
        {
            SingletonGetter = singletonGetter;
        }

        public async Task<IContent> Get(string id)
        {
            return await SingletonGetter.GetAsync(id).ConfigureAwait(false);
        }
    }
}
