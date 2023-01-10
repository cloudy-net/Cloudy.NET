using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntitySupport.Internal;
using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.SingletonSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.AssemblySupport;

namespace Cloudy.CMS.BlockSupport
{
    public record BlockTypeCreator(IEntityTypeProvider EntityTypeProvider, IPropertyDefinitionProvider PropertyDefinitionProvider, IAssemblyProvider AssemblyProvider) : IBlockTypeCreator
    {
        public IEnumerable<Type> Create()
        {
            var explicitBlockTypes = EntityTypeProvider
                .GetAll()
                .SelectMany(t => PropertyDefinitionProvider.GetFor(t.Name))
                .Where(p => p.Block)
                .Select(p => p.Type)
                .ToList()
                .AsReadOnly();

            return AssemblyProvider.GetAll()
                .SelectMany(a => a.Types)
                .Where(t => explicitBlockTypes.Any(b => t.IsAssignableTo(b)))
                .ToList()
                .AsReadOnly();
        }
    }
}
