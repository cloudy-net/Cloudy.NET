using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class GetUrlController
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IContentGetter ContentGetter { get; }
        IUrlProvider UrlProvider { get; }
        IContentRouteMatcher ContentRouteMatcher { get; }

        public GetUrlController(IContentTypeProvider contentTypeProvider, IPrimaryKeyConverter primaryKeyConverter, IContentGetter contentGetter, IUrlProvider urlProvider, IContentRouteMatcher contentRouteMatcher)
        {
            ContentTypeProvider = contentTypeProvider;
            PrimaryKeyConverter = primaryKeyConverter;
            ContentGetter = contentGetter;
            UrlProvider = urlProvider;
            ContentRouteMatcher = contentRouteMatcher;
        }

        [HttpPost]
        public async Task<IEnumerable<string>> GetUrl([FromBody] GetUrlRequestBody data)
        {
            var contentType = ContentTypeProvider.Get(data.ContentTypeId);

            var content = await ContentGetter.GetAsync(data.ContentTypeId, PrimaryKeyConverter.Convert(data.KeyValues, data.ContentTypeId)).ConfigureAwait(false);

            var contentRouteSegment = await UrlProvider.GetAsync(content).ConfigureAwait(false);

            var contentRoutes = ContentRouteMatcher.GetFor(contentType);

            var result = new List<string>();

            foreach(var contentRoute in contentRoutes)
            {
                result.Add(contentRoute.Apply(contentRouteSegment));
            }

            return result.AsReadOnly();
        }

        public class GetUrlRequestBody
        {
            public JsonElement[] KeyValues { get; set; }
            public string ContentTypeId { get; set; }
        }
    }
}
