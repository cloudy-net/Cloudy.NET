using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport
{
    public class ContentTypeActionModuleProvider : IContentTypeActionModuleProvider
    {
        IDictionary<string, IEnumerable<string>> ModulesByContentTypeId { get; }

        public ContentTypeActionModuleProvider(IContentTypeActionModuleCreator contentTypeActionModuleCreator)
        {
            ModulesByContentTypeId = contentTypeActionModuleCreator.Create().GroupBy(m => m.ContentTypeId).ToDictionary(g => g.Key, g => (IEnumerable<string>)g.Select(m => m.ModulePath).ToList().AsReadOnly());
        }

        public IEnumerable<string> GetContentTypeActionModulesFor(string contentTypeId)
        {
            if (!ModulesByContentTypeId.ContainsKey(contentTypeId))
            {
                return Enumerable.Empty<string>();
            }

            return ModulesByContentTypeId[contentTypeId];
        }
    }
}
