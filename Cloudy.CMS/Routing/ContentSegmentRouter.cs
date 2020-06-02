using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;

namespace Cloudy.CMS.Routing
{
    public class ContentSegmentRouter : IContentSegmentRouter
    {
        IDocumentFinder DocumentFinder { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentDeserializer ContentDeserializer { get; }

        public ContentSegmentRouter(IDocumentFinder documentFinder, IContentTypeProvider contentTypeRepository, IContentDeserializer contentDeserializer)
        {
            DocumentFinder = documentFinder;
            ContentTypeRepository = contentTypeRepository;
            ContentDeserializer = contentDeserializer;
        }

        public IContent RouteContentSegment(string parentId, string segment, string language)
        {
            var document = DocumentFinder.Find(ContainerConstants.Content).WhereEquals<IHierarchical, string>(x => x.ParentId, parentId).WhereEquals<IRoutable, string>(x => x.UrlSegment, segment).GetResultAsync().Result.FirstOrDefault();

            if (document == null)
            {
                return null;
            }

            var contentType = ContentTypeRepository.Get(document.GlobalFacet.Interfaces["IContent"].Properties["ContentTypeId"] as string);

            return ContentDeserializer.Deserialize(document, contentType, language);
        }
    }
}
