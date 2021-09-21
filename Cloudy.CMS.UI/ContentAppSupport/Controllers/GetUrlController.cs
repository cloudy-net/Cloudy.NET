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
using Newtonsoft.Json;
using System.Linq;

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

        public async Task<IEnumerable<string>> GetUrl(string[] keyValues, string contentTypeId)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);

            var content = await ContentGetter.GetAsync(contentTypeId, PrimaryKeyConverter.Convert(keyValues.Select(JsonConvert.DeserializeObject), contentTypeId)).ConfigureAwait(false);

            var contentRouteSegment = await UrlProvider.GetAsync(content).ConfigureAwait(false);

            var contentRoutes = ContentRouteMatcher.GetFor(contentType);

            var result = new List<string>();

            foreach(var contentRoute in contentRoutes)
            {
                result.Add(contentRoute.Apply(contentRouteSegment));
            }

            return result.AsReadOnly();
        }
    }
}
