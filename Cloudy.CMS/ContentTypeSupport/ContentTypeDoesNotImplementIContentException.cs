using System;
using System.Runtime.Serialization;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class ContentTypeDoesNotImplementIContentException : Exception
    {
        public ContentTypeDoesNotImplementIContentException(Type type, string contentTypeId) : base($"Type {type} was marked with [ContentType(\"{contentTypeId}\")], but does not implement required interface IContent") { }
    }
}