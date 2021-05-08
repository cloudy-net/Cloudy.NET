using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
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
    public class ContentGetterController
    {
        IContentGetter ContentGetter { get; }

        public ContentGetterController(IContentGetter contentGetter)
        {
            ContentGetter = contentGetter;
        }

        public async Task<object> Get(string contentId, string contentTypeId)
        {
            return await ContentGetter.GetAsync(contentTypeId, contentId).ConfigureAwait(false);
        }
    }
}
