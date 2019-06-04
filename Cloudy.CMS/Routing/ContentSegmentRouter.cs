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
        IContainerProvider ContainerProvider { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentDeserializer ContentDeserializer { get; }

        public ContentSegmentRouter(IContainerProvider containerProvider, IContentTypeProvider contentTypeRepository, IContentDeserializer contentDeserializer)
        {
            ContainerProvider = containerProvider;
            ContentTypeRepository = contentTypeRepository;
            ContentDeserializer = contentDeserializer;
        }

        public IContent RouteContentSegment(string parentId, string segment, string language)
        {
            var document = ContainerProvider.Get(ContainerConstants.Content).Find(
                Builders<Document>.Filter.And(
                    Builders<Document>.Filter.Eq(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.IHierarchical.Properties.ParentId"), parentId),
                    segment != null ?
                    Builders<Document>.Filter.Eq(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.IRoutable.Properties.UrlSegment"), segment) :
                    Builders<Document>.Filter.And(
                        Builders<Document>.Filter.Exists(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.IRoutable.Properties.UrlSegment")),
                        Builders<Document>.Filter.Eq(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.IRoutable.Properties.UrlSegment"), segment)
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
