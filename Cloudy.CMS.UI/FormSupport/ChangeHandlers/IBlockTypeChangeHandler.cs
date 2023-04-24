using Cloudy.CMS.UI.FormSupport.Changes;

namespace Cloudy.CMS.UI.FormSupport.ChangeHandlers
{
    public interface IBlockTypeChangeHandler
    {
        void SetType(object entity, BlockTypeChange change);
    }
}