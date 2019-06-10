using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;

namespace Cloudy.CMS.Mvc.Routing
{
    public class UrlProvider : IUrlProvider
    {
        IContentGetter ContentGetter { get; }
        IAncestorLinkProvider AncestorsRepository { get; }

        public UrlProvider(IAncestorLinkProvider ancestorsRepository, IContentGetter contentGetter)
        {
            AncestorsRepository = ancestorsRepository;
            ContentGetter = contentGetter;
        }

        public string Get(IContent content)
        {
            var routable = content as IRoutable;

            if (routable == null)
            {
                return null;
            }

            var hierarchical = content as IHierarchical;

            if (hierarchical == null)
            {
                if(routable.UrlSegment == null)
                {
                    return "/";
                }

                return routable.UrlSegment;
            }

            var languageSpecific = content as ILanguageSpecific;

            var allContent = AncestorsRepository.GetAncestorLinks(content.Id).Reverse().Select(id => ContentGetter.Get<IContent>(id, languageSpecific?.Language));

            if (allContent.Any(c => !(c is IRoutable)))
            {
                return null;
            }

            var segments = allContent.Cast<IRoutable>().Select(c => c.UrlSegment).ToList();

            if(segments.Any() && segments.First() == null)
            {
                segments = segments.Skip(1).ToList();
            }

            if (segments.Contains(null))
            {
                return null;
            }
            
            segments.Add(routable.UrlSegment);

            return "/" + string.Join("/", segments);
        }
    }
}
