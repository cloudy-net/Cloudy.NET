using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.ContextSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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

            var result = new EntityTypeCreator(contextDescriptorProvider).Create();

            var actual = result.Single().Name;

            Assert.Equal(expected, actual);
        }

        public class MyClass<T1, T2> { }
        public class ClassA { }
        public class ClassB { }
    }
}
