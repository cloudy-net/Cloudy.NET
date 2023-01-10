using Cloudy.CMS.AssemblySupport;
using Cloudy.CMS.BlockSupport;
using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.PropertyDefinitionSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class BlockTypeCreatorTests
    {
        [Fact]
        public void SupportsInterfaces()
        {
            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(e => e.GetAll()).Returns(new List<EntityTypeDescriptor> { new EntityTypeDescriptor(nameof(PropertyOwner), typeof(PropertyOwner)) });

            var propertyDefinitionProvider = Mock.Of<IPropertyDefinitionProvider>();
            Mock.Get(propertyDefinitionProvider).Setup(p => p.GetFor(nameof(PropertyOwner))).Returns(new List<PropertyDefinitionDescriptor> { new PropertyDefinitionDescriptor("test", typeof(Interface), null, null, null, false, false, false, true) });

            var assemblyProvider = Mock.Of<IAssemblyProvider>();
            Mock.Get(assemblyProvider).Setup(a => a.GetAll()).Returns(new List<AssemblyWrapper> { new AssemblyWrapper(new List<Type> { typeof(TypeA), typeof(TypeB), typeof(TypeC) }) });

            var result = new BlockTypeCreator(entityTypeProvider, propertyDefinitionProvider, assemblyProvider).Create();

            Assert.Equal(new List<BlockTypeDescriptor> { new BlockTypeDescriptor(nameof(TypeA), typeof(TypeA)), new BlockTypeDescriptor(nameof(TypeB), typeof(TypeB)) }, result);
        }

        class PropertyOwner { }
        interface Interface { }
        class TypeA : Interface { }
        class TypeB : Interface { }
        class TypeC { }
    }
}
