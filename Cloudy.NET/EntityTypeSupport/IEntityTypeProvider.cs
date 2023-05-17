using System;
using System.Collections.Generic;

namespace Cloudy.NET.EntityTypeSupport
{
    public interface IEntityTypeProvider
    {
        EntityTypeDescriptor Get(Type type);
        EntityTypeDescriptor Get(string name);
        IEnumerable<EntityTypeDescriptor> GetAll();
    }
}