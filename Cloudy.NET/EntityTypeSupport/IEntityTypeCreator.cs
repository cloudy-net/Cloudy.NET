using System.Collections.Generic;

namespace Cloudy.NET.EntityTypeSupport
{
    public interface IEntityTypeCreator
    {
        IEnumerable<EntityTypeDescriptor> Create();
    }
}