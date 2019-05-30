using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using MongoDB.Driver;

namespace Cloudy.CMS.Routing
{
    public class ContentSegmentRouter : IContentSegmentRouter
    {
        IDocumentRepository DocumentRepository { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentDeserializer ContentDeserializer { get; }

        public ContentSegmentRouter(IDocumentRepository documentRepository, IContentTypeProvider contentTypeRepository, IContentDeserializer contentDeserializer)
        {
            DocumentRepository = documentRepository;
            ContentTypeRepository = contentTypeRepository;
            ContentDeserializer = contentDeserializer;
        }

        public IContent RouteContentSegment(string parentId, string segment, string language)
        {
            var document = DocumentRepository.Documents.Find(
                Builders<Document>.Filter.And(
                    Builders<Document>.Filter.Eq(d => d.GlobalFacet.Interfaces["IHierarchical"].Properties["ParentId"], parentId),
                    segment != null ?
                    Builders<Document>.Filter.Eq(d => d.GlobalFacet.Interfaces["IRoutable"].Properties["UrlSegment"], segment) :
                    Builders<Document>.Filter.And(
                        Builders<Document>.Filter.Exists(d => d.GlobalFacet.Interfaces["IRoutable"].Properties["UrlSegment"]),
                        Builders<Document>.Filter.Eq(d => d.GlobalFacet.Interfaces["IRoutable"].Properties["UrlSegment"], segment)
                    )
                )
            ).FirstOrDefault();

            if (document == null)
            {
                return null;
            }

            var contentType = ContentTypeRepository.Get(document.GlobalFacet.Interfaces["IContent"].Properties["ContentTypeId"] as string);

            return ContentDeserializer.Deserialize(document, contentType, language);
        }
    }
}
