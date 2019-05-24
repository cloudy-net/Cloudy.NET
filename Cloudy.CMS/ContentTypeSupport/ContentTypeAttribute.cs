using System;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class ContentTypeAttribute : Attribute
    {
        public string Id { get; }

        public ContentTypeAttribute(string id)
        {
            Id = id;
        }
    }
}