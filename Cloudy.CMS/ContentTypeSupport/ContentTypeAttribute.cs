using System;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class ContentTypeAttribute : Attribute
    {
        public string Id { get; }

        [Obsolete("You need to supply an Id - use the parameterless constructor only to get a suggestion")]
        public ContentTypeAttribute() : this(null) { }

        public ContentTypeAttribute(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception($"Id must be provided when using [ContentType(...)]. How about `{Guid.NewGuid()}` ?");
            }

            Id = id;
        }
    }
}