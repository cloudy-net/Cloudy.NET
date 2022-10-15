using System;

namespace Cloudy.CMS.EntitySupport.Reference
{
    public interface IReferenceDeserializer
    {
        object[] Deserialize(Type contentType, string reference);
    }
}