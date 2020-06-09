using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport.GroupSupport
{
    public class ContentTypeGroupProvider : IContentTypeGroupProvider
    {
        IEnumerable<ContentTypeGroupDescriptor> ContentTypeGroups { get; }

        public ContentTypeGroupProvider(IContentTypeGroupCreator contentTypeGroupCreator)
        {
            ContentTypeGroups = contentTypeGroupCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<ContentTypeGroupDescriptor> GetAll()
        {
            return ContentTypeGroups;
        }
    }
}
