using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ChildrenGetter : IChildrenGetter
    {
        IContainerProvider ContainerProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContentDeserializer ContentDeserializer { get; }

        public ChildrenGetter(IContainerProvider containerProvider, IContentTypeProvider contentTypeProvider, IContentDeserializer contentDeserializer)
        {
            ContainerProvider = containerProvider;
            ContentTypeProvider = contentTypeProvider;
            ContentDeserializer = contentDeserializer;
        }

        public IEnumerable<T> GetChildren<T>(string id, string language) where T : class
        {
            var contentTypes = ContentTypeProvider.GetAll().Where(t => typeof(T).IsAssignableFrom(t.Type));

            var documents = ContainerProvider
                .Get(ContainerConstants.Content)
                .Find(
                    Builders<Document>.Filter.And(
                        Builders<Document>.Filter.Eq(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.IHierarchical.Properties.ParentId"), id),
                        Builders<Document>.Filter.In(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.IContent.Properties.ContentTypeId"), contentTypes.Select(t => t.Id))
                    )
                )
                .ToList();

            var result = new List<T>();

            foreach (var document in documents)
            {
                var contentTypeId = document.GlobalFacet.Interfaces["IContent"].Properties["ContentTypeId"] as string;

                result.Add((T)ContentDeserializer.Deserialize(document, ContentTypeProvider.Get(contentTypeId), language));
            }

            return result.AsReadOnly();
        }
    }
}
