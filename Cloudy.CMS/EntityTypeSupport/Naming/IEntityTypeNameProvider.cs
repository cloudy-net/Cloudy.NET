using System;

namespace Cloudy.CMS.EntityTypeSupport.Naming
{
    public interface IEntityTypeNameProvider
    {
        EntityTypeName Get(Type type);
    }
}