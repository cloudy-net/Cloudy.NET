using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport.GroupSupport
{
    public class ContentTypeGroupAttribute : Attribute
    {
        public string Id { get; }

        public ContentTypeGroupAttribute(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception($"Id must be provided when using [ContentTypeGroup(...)]. How about {Guid.NewGuid()} ?");
            }

            Id = id;
        }
    }
}
