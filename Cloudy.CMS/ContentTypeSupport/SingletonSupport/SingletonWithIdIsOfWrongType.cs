using System;
using System.Runtime.Serialization;

namespace Cloudy.CMS.ContentTypeSupport.SingletonSupport
{
    [Serializable]
    public class SingletonWithIdIsOfWrongType : Exception
    {
        public SingletonWithIdIsOfWrongType(string id, ContentTypeDescriptor contentType, Type wrongType, string wrongContentTypeId) : base($"Singleton {id} should be of type {contentType.Type} ({contentType.Id}), but was {wrongType} ({wrongContentTypeId})") { }
    }
}