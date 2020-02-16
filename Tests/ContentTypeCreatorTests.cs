using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Moq;
using Poetry;
using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class ContentTypeCreatorTests
    {
        [Fact]
        public void IgnoresAbstractClasses()
        {
            var component = new ComponentDescriptor("ipsum", new AssemblyWrapper(new List<Type> { typeof(Class_A_Abstract) }));
            var componentProvider = Mock.Of<IComponentProvider>();
            Mock.Get(componentProvider).Setup(p => p.GetAll()).Returns(new List<ComponentDescriptor> { component });

            var result = new ContentTypeCreator(componentProvider).Create();

            Assert.Empty(result);
        }

        [ContentType("lorem")]
        public abstract class Class_A_Abstract : IContent
        {
            public string Id { get; set; }
            public string ContentTypeId { get; set; }
        }
    }
}
