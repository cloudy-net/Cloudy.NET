using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Moq;
using Poetry;
using Poetry.ComponentSupport;
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
            var component = new ComponentDescriptor("ipsum", new AssemblyWrapper(new List<Type> { typeof(Class_A_Abstract) }));
            var componentProvider = Mock.Of<IComponentProvider>();
            Mock.Get(componentProvider).Setup(p => p.GetAll()).Returns(new List<ComponentDescriptor> { component });

            var result = new ContentTypeCreator(componentProvider).Create();

            Assert.Empty(result);
        }

        [Fact]
        public void IncludesAbstractClassIfSingleSubclassExists()
        {
            var component = new ComponentDescriptor("ipsum", new AssemblyWrapper(new List<Type> { typeof(Class_B_Extends_A) }));
            var componentProvider = Mock.Of<IComponentProvider>();
            Mock.Get(componentProvider).Setup(p => p.GetAll()).Returns(new List<ComponentDescriptor> { component });

            var result = new ContentTypeCreator(componentProvider).Create();

            Assert.Single(result);

            var contentType = result.Single();

            Assert.Equal("lorem", contentType.Id);
            Assert.Same(typeof(Class_B_Extends_A), contentType.Type);
        }

        [ContentType("lorem")]
        public abstract class Class_A_Abstract : IContent
        {
            public string Id { get; set; }
            public string ContentTypeId { get; set; }
        }

        public class Class_B_Extends_A : Class_A_Abstract { }
    }
}
