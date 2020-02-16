using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class ContentDeserializerTests
    {
        [Fact]
        public void HandlesJArray()
        {
            var contentType = new ContentTypeDescriptor("lorem", typeof(MyContent), "ipsum");
            var property = new PropertyDefinitionDescriptor(nameof(MyContent.ArrayProperty), typeof(List<string>), o => ((MyContent)o).ArrayProperty, (o, v) => ((MyContent)o).ArrayProperty = (List<string>)v, Enumerable.Empty<object>());
            var propertyDefinitionProvider = Mock.Of<IPropertyDefinitionProvider>();
            Mock.Get(propertyDefinitionProvider).Setup(p => p.GetFor(contentType.Id)).Returns(new List<PropertyDefinitionDescriptor> { property });
            var value = "dolor";
            var document = new Document { GlobalFacet = new DocumentFacet { Properties = new Dictionary<string, object> { [nameof(MyContent.ArrayProperty)] = new JArray { value } } } };
            var contentTypeCoreInterfaceProvider = Mock.Of<IContentTypeCoreInterfaceProvider>();
            Mock.Get(contentTypeCoreInterfaceProvider).Setup(p => p.GetFor(It.IsAny<string>())).Returns(Enumerable.Empty<CoreInterfaceDescriptor>());

            var result = (MyContent)new ContentDeserializer(propertyDefinitionProvider, contentTypeCoreInterfaceProvider).Deserialize(document, contentType, null);

            Assert.Equal(new List<string> { value }, result.ArrayProperty);
        }

        [Fact]
        public void HandlesJObject()
        {
            var contentType = new ContentTypeDescriptor("lorem", typeof(MyContent), "ipsum");
            var property = new PropertyDefinitionDescriptor(nameof(MyContent.ObjectProperty), typeof(MyItem), o => ((MyContent)o).ObjectProperty, (o, v) => ((MyContent)o).ObjectProperty = (MyItem)v, Enumerable.Empty<object>());
            var propertyDefinitionProvider = Mock.Of<IPropertyDefinitionProvider>();
            Mock.Get(propertyDefinitionProvider).Setup(p => p.GetFor(contentType.Id)).Returns(new List<PropertyDefinitionDescriptor> { property });
            var value = "dolor";
            var document = new Document { GlobalFacet = new DocumentFacet { Properties = new Dictionary<string, object> { [nameof(MyContent.ObjectProperty)] = new JObject { ["value"] = value } } } };
            var contentTypeCoreInterfaceProvider = Mock.Of<IContentTypeCoreInterfaceProvider>();
            Mock.Get(contentTypeCoreInterfaceProvider).Setup(p => p.GetFor(It.IsAny<string>())).Returns(Enumerable.Empty<CoreInterfaceDescriptor>());

            var result = (MyContent)new ContentDeserializer(propertyDefinitionProvider, contentTypeCoreInterfaceProvider).Deserialize(document, contentType, null);

            Assert.NotNull(result.ObjectProperty);
            Assert.Equal(value, result.ObjectProperty.Value);
        }

        public class MyContent : IContent
        {
            public string Id { get; set; }
            public string ContentTypeId { get; set; }
            public List<string> ArrayProperty { get; set; }
            public MyItem ObjectProperty { get; set; }
        }

        public class MyItem
        {
            public string Value { get; set; }
        }
    }
}
