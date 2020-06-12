using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport.GroupSupport
{
    public class ContentTypeGroupMatcher : IContentTypeGroupMatcher
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentTypeGroupProvider ContentTypeGroupProvider { get; }

        public ContentTypeGroupMatcher(IContentTypeProvider contentTypeProvider, IContentTypeGroupProvider contentTypeGroupProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentTypeGroupProvider = contentTypeGroupProvider;
        }

        public IEnumerable<ContentTypeGroupDescriptor> GetContentTypeGroupsFor(string contentTypeId)
        {
            var result = new List<ContentTypeGroupDescriptor>();

            var contentType = ContentTypeProvider.Get(contentTypeId);

            foreach (var contentTypeGroup in ContentTypeGroupProvider.GetAll())
            {
                if (contentTypeGroup.Type.IsAssignableFrom(contentType.Type))
                {
                    result.Add(contentTypeGroup);
                    break;
                }
            }

            return result.AsReadOnly();
        }

        public IEnumerable<ContentTypeDescriptor> GetContentTypesFor(string contentTypeGroupId)
        {
            var result = new List<ContentTypeDescriptor>();

            var contentTypeGroup = ContentTypeGroupProvider.Get(contentTypeGroupId);

            foreach(var contentType in ContentTypeProvider.GetAll())
            {
                if (contentTypeGroup.Type.IsAssignableFrom(contentType.Type))
                {
                    result.Add(contentType);
                }
            }

            return result.AsReadOnly();
        }
    }
}
