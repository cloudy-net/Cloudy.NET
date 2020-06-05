using Cloudy.CMS.ComposableSupport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    public interface IItemProvider : IComposable
    {
        Task<Item> Get(string type, string value);
        Task<IEnumerable<Item>> GetAll(string type);
    }
}