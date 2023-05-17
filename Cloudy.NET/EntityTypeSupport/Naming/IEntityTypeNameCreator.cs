using System;
using System.Collections.Generic;

namespace Cloudy.NET.EntityTypeSupport.Naming
{
    public interface IEntityTypeNameCreator
    {
        IEnumerable<EntityTypeName> Create();
    }
}