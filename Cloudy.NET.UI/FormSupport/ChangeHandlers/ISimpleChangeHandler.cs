using Cloudy.NET.UI.FormSupport.Changes;

namespace Cloudy.NET.UI.FormSupport.ChangeHandlers
{
    public interface ISimpleChangeHandler
    {
        void SetValue(object entity, SimpleChange change);
    }
}