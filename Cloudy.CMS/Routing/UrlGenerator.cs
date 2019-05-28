using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;

namespace Cloudy.CMS.Mvc.Routing
{
    public class UrlGenerator : IUrlGenerator
    {
        IContentGetter ContentGetter { get; }
        IAncestorLinkProvider AncestorsRepository { get; }

        public UrlGenerator(IAncestorLinkProvider ancestorsRepository, IContentGetter contentGetter)
        {
            AncestorsRepository = ancestorsRepository;
            ContentGetter = contentGetter;
        }

        public string Generate(IContent content)
        {
            var navigatable = content as IRoutable;

            if (navigatable == null)
            {
                return null;
            }

            var hierarchical = content as IHierarchical;

            if (hierarchical == null)
            {
                return navigatable.UrlSegment;
            }

            var language = content is ILanguageSpecific ? ((ILanguageSpecific)content).Language : DocumentLanguageConstants.Global;

            var allContent = AncestorsRepository.GetAncestorLinks(content.Id).Select(id => ContentGetter.Get<IContent>(id, language));

            if (allContent.Any(c => !(c is IRoutable)))
            {
                return null;
            }

            var segments = allContent.Cast<IRoutable>().Select(c => c.UrlSegment).ToList();

            segments.Add(navigatable.UrlSegment);

            return string.Join("/", segments);
        }
    }
}
