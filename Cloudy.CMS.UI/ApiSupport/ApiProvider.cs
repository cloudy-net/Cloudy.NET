using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poetry.ComponentSupport;

namespace Poetry.UI.ApiSupport
{
    public class ApiProvider : IApiProvider
    {
        IDictionary<string, IEnumerable<Api>> Apis { get; }

        public ApiProvider(IComponentProvider componentProvider, IApiCreator apiCreator)
        {
            Apis = componentProvider.GetAll().ToDictionary(c => c.Id, c => (IEnumerable<Api>)apiCreator.Create(c).ToList().AsReadOnly());
        }

        public IEnumerable<Api> GetAllFor(ComponentDescriptor component)
        {
            return Apis[component.Id];
        }
    }
}
