using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ActionSupport
{
    public class ContentActionModuleProvider : IContentActionModuleProvider
    {
        IDictionary<string, IEnumerable<string>> ModulesByContentTypeId { get; }

        public ContentActionModuleProvider(IContentActionModuleCreator contentActionModuleCreator)
        {
            ModulesByContentTypeId = contentActionModuleCreator.Create().GroupBy(m => m.ContentTypeId).ToDictionary(g => g.Key, g => (IEnumerable<string>)g.Select(m => m.ModulePath).ToList().AsReadOnly());
        }

        public IEnumerable<string> GetModulesFor(string contentTypeId)
        {
            if (!ModulesByContentTypeId.ContainsKey(contentTypeId))
            {
                return Enumerable.Empty<string>();
            }

            return ModulesByContentTypeId[contentTypeId];
        }
    }
}
