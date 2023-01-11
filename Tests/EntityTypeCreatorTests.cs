using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.ContextSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Cloudy.CMS.AssemblySupport;
using Cloudy.CMS.PropertyDefinitionSupport;

namespace Tests
{
    public class EntityTypeCreatorTests
    {
        [Fact]
        public void GeneratesNamesOfGenericTypes()
        {
            var expected = $"MyClass<ClassA,ClassB>";

            var contextDescriptorProvider = Mock.Of<IContextDescriptorProvider>();

            Mock.Get(contextDescriptorProvider).Setup(c => c.GetAll()).Returns(new List<ContextDescriptor> { new ContextDescriptor(typeof(string), new List<DbSetDescriptor> { new DbSetDescriptor(typeof(MyClass<ClassA, ClassB>), null) }) });

            var result = new EntityTypeCreator(contextDescriptorProvider, Mock.Of<IAssemblyProvider>()).Create();

            var actual = result.Single().Name;

            Assert.Equal(expected, actual);
        }

        public class MyClass<T1, T2> { }
        public class ClassA { }
        public class ClassB { }

        [Fact]
        public void SupportsBlockInterfaces()
        {
            var contextDescriptorProvider = Mock.Of<IContextDescriptorProvider>();
            Mock.Get(contextDescriptorProvider).Setup(c => c.GetAll()).Returns(new List<ContextDescriptor> { new ContextDescriptor(typeof(string), new List<DbSetDescriptor> { new DbSetDescriptor(typeof(PropertyOwner), null) }) });

            var assemblyProvider = Mock.Of<IAssemblyProvider>();
            Mock.Get(assemblyProvider).Setup(a => a.GetAll()).Returns(new List<AssemblyWrapper> { new AssemblyWrapper(new List<Type> { typeof(TypeA), typeof(TypeB), typeof(TypeC) }) });

            var result = new EntityTypeCreator(contextDescriptorProvider, assemblyProvider).Create();

            Assert.Equal(new List<EntityTypeDescriptor> { new EntityTypeDescriptor(nameof(PropertyOwner), typeof(PropertyOwner), true), new EntityTypeDescriptor(nameof(TypeA), typeof(TypeA)), new EntityTypeDescriptor(nameof(TypeB), typeof(TypeB)) }, result);
        }

        class PropertyOwner { public Interface Interface { get; set; } }
        interface Interface { }
        class TypeA : Interface { }
        class TypeB : Interface { }
        class TypeC { }
    }
}
