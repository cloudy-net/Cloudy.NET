using System;
using System.Collections.Generic;

namespace Cloudy.CMS.EntityTypeSupport.Naming
{
    public interface IEntityTypeNameCreator
    {
        IEnumerable<EntityTypeName> Create();
    }
}