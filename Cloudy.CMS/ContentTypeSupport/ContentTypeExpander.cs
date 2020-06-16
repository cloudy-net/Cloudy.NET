using Cloudy.CMS.ContentTypeSupport.GroupSupport;
using System;
using System.Collections.Generic;
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

        public IEnumerable<ContentTypeDescriptor> Expand(string typeOrGroupId)
        {
            var contentType = ContentTypeProvider.Get(typeOrGroupId);

            if(contentType != null)
            {
                return new List<ContentTypeDescriptor> { contentType }.AsReadOnly();
            }

            var contentTypeGroup = ContentTypeGroupProvider.Get(typeOrGroupId);

            if(contentTypeGroup != null)
            {
                return ContentTypeGroupMatcher.GetContentTypesFor(contentTypeGroup.Id);
            }

            return null;
        }
    }
}
