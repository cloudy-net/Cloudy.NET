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
        Dictionary<string, ContentTypeDescriptor> ContentTypesByName { get; }

        public ContentTypeProvider(IContentTypeCreator contentTypeCreator)
        {
            ContentTypes = contentTypeCreator.Create().ToList().AsReadOnly();
            ContentTypesByType = ContentTypes.ToDictionary(t => t.Type, t => t);
            ContentTypesByName = ContentTypes.ToDictionary(t => t.Name, t => t);
        }

        public ContentTypeDescriptor Get(Type type)
        {
            return GetMostSpecificAssignableFrom(ContentTypesByType, type);
        }

        public ContentTypeDescriptor Get(string name)
        {
            if (!ContentTypesByName.ContainsKey(name))
            {
                return null;
            }

            return ContentTypesByName[name];
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
