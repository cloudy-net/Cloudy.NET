using System;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class ContentTypeAttribute : Attribute
    {
        public string Id { get; }

        public ContentTypeAttribute(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception($"Id must be provided when using [ContentType(...)]. How about {Guid.NewGuid()} ?");
            }

            Id = id;
        }
    }
}