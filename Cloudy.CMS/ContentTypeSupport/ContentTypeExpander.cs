using Cloudy.CMS.ContentTypeSupport.GroupSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class ContentTypeExpander : IContentTypeExpander
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentTypeGroupProvider ContentTypeGroupProvider { get; }
        IContentTypeGroupMatcher ContentTypeGroupMatcher { get; }

        public ContentTypeExpander(IContentTypeProvider contentTypeProvider, IContentTypeGroupProvider contentTypeGroupProvider, IContentTypeGroupMatcher contentTypeGroupMatcher)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentTypeGroupProvider = contentTypeGroupProvider;
            ContentTypeGroupMatcher = contentTypeGroupMatcher;
        }

        public IEnumerable<ContentTypeDescriptor> Expand(string typeOrGroupIdOrTypeName)
        {
            var contentType = ContentTypeProvider.Get(typeOrGroupIdOrTypeName);

            if(contentType != null)
            {
                return new List<ContentTypeDescriptor> { contentType }.AsReadOnly();
            }

            var contentTypeGroup = ContentTypeGroupProvider.Get(typeOrGroupIdOrTypeName);

            if(contentTypeGroup != null)
            {
                return ContentTypeGroupMatcher.GetContentTypesFor(contentTypeGroup.Id);
            }

            var contentTypesByTypeName = ContentTypeProvider.GetAll().Where(t => t.Type.Name == typeOrGroupIdOrTypeName || t.Type.GetInterfaces().Any(i => i.Name == typeOrGroupIdOrTypeName)).ToList().AsReadOnly();

            if (contentTypesByTypeName.Any())
            {
                return contentTypesByTypeName;
            }

            return null;
        }
    }
}
