using System;

namespace Cloudy.CMS.EntitySupport.Reference
{
    public interface IReferenceDeserializer
    {
        object[] Get(Type contentType, string reference, bool simpleKey);
    }
}