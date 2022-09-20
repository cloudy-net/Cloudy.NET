using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Moq;
using Cloudy.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class ContentTypeCreatorTests
    {
        [Fact]
        public void ExcludesAbstractClass()
        {
            var assemblyProvider = Mock.Of<IAssemblyProvider>();
            Mock.Get(assemblyProvider).Setup(p => p.GetAll()).Returns(new List<AssemblyWrapper> { new AssemblyWrapper(new List<Type> { typeof(Class_A_Abstract) }) });

            var result = new ContentTypeCreator(assemblyProvider).Create();

            Assert.Empty(result);
        }

        [Fact]
        public void IncludesAbstractClassIfSingleSubclassExists()
        {
            var assemblyProvider = Mock.Of<IAssemblyProvider>();
            Mock.Get(assemblyProvider).Setup(p => p.GetAll()).Returns(new List<AssemblyWrapper> { new AssemblyWrapper(new List<Type> { typeof(Class_B_Extends_A) }) });

            var result = new ContentTypeCreator(assemblyProvider).Create();

            Assert.Single(result);

            var contentType = result.Single();

            Assert.Equal("lorem", contentType.Id);
            Assert.Same(typeof(Class_B_Extends_A), contentType.Type);
        }

        [ContentType]
        public abstract class Class_A_Abstract
        {
            public string Id { get; set; }
        }

        public class Class_B_Extends_A : Class_A_Abstract { }
    }
}
