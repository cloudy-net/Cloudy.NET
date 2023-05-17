using System;

namespace Cloudy.NET.EntityTypeSupport.Naming
{
    public interface IEntityTypeNameProvider
    {
        EntityTypeName Get(Type type);
    }
}