using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport.GroupSupport
{
    public class ContentTypeGroupProvider : IContentTypeGroupProvider
    {
        IEnumerable<ContentTypeGroupDescriptor> ContentTypeGroups { get; }
        IDictionary<string, ContentTypeGroupDescriptor> ContentTypeGroupsById { get; }

        public ContentTypeGroupProvider(IContentTypeGroupCreator contentTypeGroupCreator)
        {
            ContentTypeGroups = contentTypeGroupCreator.Create().ToList().AsReadOnly();
            ContentTypeGroupsById = ContentTypeGroups.ToDictionary(g => g.Id, g => g);
        }

        public IEnumerable<ContentTypeGroupDescriptor> GetAll()
        {
            return ContentTypeGroups;
        }

        public ContentTypeGroupDescriptor Get(string contentTypeGroupId)
        {
            if (!ContentTypeGroupsById.ContainsKey(contentTypeGroupId))
            {
                return null;
            }

            return ContentTypeGroupsById[contentTypeGroupId];
        }
    }
}
