using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    public interface IItemCreator<T>: IItemProvider where T : class
    {
        public Task CreateAsync(T model);
    }
}
