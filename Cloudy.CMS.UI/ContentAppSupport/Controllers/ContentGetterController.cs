using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class ContentGetterController
    {
        IContentGetter ContentGetter { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }

        public ContentGetterController(IContentGetter contentGetter, IPrimaryKeyConverter primaryKeyConverter)
        {
            ContentGetter = contentGetter;
            PrimaryKeyConverter = primaryKeyConverter;
        }

        [HttpPost]
        public async Task<object> Get([FromBody] GetContentRequestBody data)
        {
            return await ContentGetter.GetAsync(data.ContentTypeId, PrimaryKeyConverter.Convert(data.KeyValues, data.ContentTypeId)).ConfigureAwait(false);
        }

        public class GetContentRequestBody
        {
            public JsonElement[] KeyValues { get; set; }
            public string ContentTypeId { get; set; }
        }
    }
}
