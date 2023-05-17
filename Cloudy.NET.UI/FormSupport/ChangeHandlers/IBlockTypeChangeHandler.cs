using Cloudy.NET.UI.FormSupport.Changes;

namespace Cloudy.NET.UI.FormSupport.ChangeHandlers
{
    public interface IBlockTypeChangeHandler
    {
        void SetType(object entity, BlockTypeChange change);
    }
}