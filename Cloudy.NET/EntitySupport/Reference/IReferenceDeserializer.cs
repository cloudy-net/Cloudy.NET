using System;

namespace Cloudy.CMS.EntitySupport.Reference
{
    public interface IReferenceDeserializer
    {
        object[] Get(Type entityType, string reference, bool simpleKey);
    }
}