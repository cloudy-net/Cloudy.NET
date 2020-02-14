using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
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
            var property = new PropertyDefinitionDescriptor(nameof(MyContent.ArrayProperty), typeof(List<string>), o => ((MyContent)o).ArrayProperty, (o, v) => ((MyContent)o).ArrayProperty = (List<string>)v, Enumerable.Empty<object>());
            var contentType = new ContentTypeDescriptor("lorem", typeof(MyContent), "ipsum", new List<PropertyDefinitionDescriptor> { property }, Enumerable.Empty<CoreInterfaceDescriptor>());
            var value = "dolor";
            var document = new Document { GlobalFacet = new DocumentFacet { Properties = new Dictionary<string, object> { [nameof(MyContent.ArrayProperty)] = new JArray { value } } } };

            var result = (MyContent)new ContentDeserializer().Deserialize(document, contentType, null);

            Assert.Equal(new List<string> { value }, result.ArrayProperty);
        }

        [Fact]
        public void HandlesJObject()
        {
            var property = new PropertyDefinitionDescriptor(nameof(MyContent.ObjectProperty), typeof(MyItem), o => ((MyContent)o).ObjectProperty, (o, v) => ((MyContent)o).ObjectProperty = (MyItem)v, Enumerable.Empty<object>());
            var contentType = new ContentTypeDescriptor("lorem", typeof(MyContent), "ipsum", new List<PropertyDefinitionDescriptor> { property }, Enumerable.Empty<CoreInterfaceDescriptor>());
            var value = "dolor";
            var document = new Document { GlobalFacet = new DocumentFacet { Properties = new Dictionary<string, object> { [nameof(MyContent.ObjectProperty)] = new JObject { ["value"] = value } } } };

            var result = (MyContent)new ContentDeserializer().Deserialize(document, contentType, null);

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
