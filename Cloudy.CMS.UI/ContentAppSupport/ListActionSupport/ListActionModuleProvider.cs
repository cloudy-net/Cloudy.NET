using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ListActionSupport
{
    public class ListActionModuleProvider : IListActionModuleProvider
    {
        IDictionary<string, IEnumerable<string>> ModulesByContentTypeId { get; }

        public ListActionModuleProvider(IListActionModuleCreator listActionModuleCreator)
        {
            ModulesByContentTypeId = listActionModuleCreator.Create().GroupBy(m => m.ContentTypeId).ToDictionary(g => g.Key, g => (IEnumerable<string>)g.Select(m => m.ModulePath).ToList().AsReadOnly());
        }

        public IEnumerable<string> GetListActionModulesFor(string contentTypeId)
        {
            if (!ModulesByContentTypeId.ContainsKey(contentTypeId))
            {
                return Enumerable.Empty<string>();
            }

            return ModulesByContentTypeId[contentTypeId];
        }
    }
}
