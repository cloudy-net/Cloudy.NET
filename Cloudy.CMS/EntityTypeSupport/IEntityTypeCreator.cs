using System.Collections.Generic;

namespace Cloudy.CMS.EntityTypeSupport
{
    public interface IEntityTypeCreator
    {
        IEnumerable<EntityTypeDescriptor> Create();
    }
}