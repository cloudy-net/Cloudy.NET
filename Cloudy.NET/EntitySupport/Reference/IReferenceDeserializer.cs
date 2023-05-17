using System;

namespace Cloudy.NET.EntitySupport.Reference
{
    public interface IReferenceDeserializer
    {
        object[] Get(Type entityType, string reference, bool simpleKey);
    }
}