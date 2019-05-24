using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.Routing
{
    public class ContentRouter : IContentRouter
    {
        IDocumentRepository DocumentRepository { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentDeserializer ContentDeserializer { get; }

        public ContentRouter(IDocumentRepository documentRepository, IContentTypeProvider contentTypeRepository, IContentDeserializer contentDeserializer)
        {
            DocumentRepository = documentRepository;
            ContentTypeRepository = contentTypeRepository;
            ContentDeserializer = contentDeserializer;
        }

        public IContent GetContentBySegment(string segment, IContent parent, string language)
        {
            var parentId = parent?.Id ?? null;
            
            return GetChildByUrlSegment<IContent>(parentId, segment, language);
        }

        T GetChildByUrlSegment<T>(string parentId, string segment, string language) where T : class
        {
            return GetChildByUrlSegmentAsync<T>(parentId, segment, language).WaitAndUnwrapException();
        }

        async Task<T> GetChildByUrlSegmentAsync<T>(string parentId, string segment, string language) where T : class
        {
            var document = (await DocumentRepository.Documents.FindAsync(
                Builders<Document>.Filter.And(
                    Builders<Document>.Filter.Eq(d => d.GlobalFacet.Interfaces["IHierarchical"].Properties["ParentId"], parentId),
                    Builders<Document>.Filter.Eq(d => d.GlobalFacet.Interfaces["INavigatable"].Properties["UrlSegment"], segment)
                )
            ).ConfigureAwait(false)).FirstOrDefault();

            if (document == null)
            {
                return null;
            }

            var contentType = ContentTypeRepository.Get(document.GlobalFacet.Interfaces["IContent"].Properties["ContentTypeId"] as string);

            return (T)ContentDeserializer.Deserialize(document, contentType, language);
        }

        public IContent RouteContent(IEnumerable<string> segments, string language)
        {
            IContent page = null;

            while (segments.Any())
            {
                page = GetContentBySegment(segments.First(), page, language);

                if (page == null)
                {
                    return null;
                }

                segments = segments.Skip(1);
            }

            return page;
        }
    }
}
