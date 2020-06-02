using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ChildrenGetter : IChildrenGetter
    {
        IDocumentFinder DocumentFinder { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContentDeserializer ContentDeserializer { get; }

        public ChildrenGetter(IDocumentFinder documentFinder, IContentTypeProvider contentTypeProvider, IContentDeserializer contentDeserializer)
        {
            DocumentFinder = documentFinder;
            ContentTypeProvider = contentTypeProvider;
            ContentDeserializer = contentDeserializer;
        }

        public IEnumerable<T> GetChildren<T>(string id, string language) where T : class
        {
            var contentTypes = ContentTypeProvider.GetAll().Where(t => typeof(T).IsAssignableFrom(t.Type));

            var documents = DocumentFinder.Find(ContainerConstants.Content).WhereEquals<IHierarchical, string>(x => x.ParentId, id).WhereIn<IContent, string>(x => x.ContentTypeId, contentTypes.Select(t => t.Id)).GetResultAsync().Result.ToList();

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
