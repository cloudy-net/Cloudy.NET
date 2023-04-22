using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport
{
    public interface IEntityChangeApplier
    {
        void Apply(object entity, EntityChange change);
    }
}