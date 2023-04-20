using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport
{
    public interface IFormEntityUpdater
    {
        void Update(object entity, IEnumerable<EntityChange> changes);
    }
}