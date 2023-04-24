using Cloudy.CMS.UI.FormSupport.Changes;

namespace Cloudy.CMS.UI.FormSupport.ChangeHandlers
{
    public interface ISimpleChangeHandler
    {
        void SetValue(object entity, SimpleChange change);
    }
}