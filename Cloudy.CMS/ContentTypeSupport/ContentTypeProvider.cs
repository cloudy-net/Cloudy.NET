using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class ContentTypeProvider : IContentTypeProvider
    {
        IEnumerable<ContentTypeDescriptor> ContentTypes { get; }
        Dictionary<Type, ContentTypeDescriptor> ContentTypesByType { get; }
        Dictionary<string, ContentTypeDescriptor> ContentTypesById { get; }

        public ContentTypeProvider(IContentTypeCreator contentTypeCreator)
        {
            ContentTypes = contentTypeCreator.Create().ToList().AsReadOnly();
            ContentTypesByType = ContentTypes.ToDictionary(t => t.Type, t => t);
            ContentTypesById = ContentTypes.ToDictionary(t => t.Id, t => t);
        }

        public ContentTypeDescriptor Get(Type type)
        {
            return GetMostSpecificAssignableFrom(ContentTypesByType, type);
        }

        public ContentTypeDescriptor Get(string id)
        {
            if (!ContentTypesById.ContainsKey(id))
            {
                return null;
            }

            return ContentTypesById[id];
        }

        public IEnumerable<ContentTypeDescriptor> GetAll()
        {
            return ContentTypes;
        }

        public static ContentTypeDescriptor GetMostSpecificAssignableFrom(Dictionary<Type, ContentTypeDescriptor> contentTypes, Type type)
        {
            if (contentTypes.ContainsKey(type))
            {
                return contentTypes[type];
            }

            var baseType = type.GetTypeInfo().BaseType;

            if (baseType == null)
            {
                return null;
            }

            return GetMostSpecificAssignableFrom(contentTypes, baseType);
        }
    }
}
