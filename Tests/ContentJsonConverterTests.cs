using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Tests
{
    public class ContentJsonConverterTests
    {
        public class ContentTypeA : InterfaceA
        {
            public string Test { get; set; } = "Lorem";
        }

        public class ContentTypeB
        {
            public IList<InterfaceA> Items { get; set; }
        }

        public class InterfaceA
        {
        }

        [Fact]
        public void SerializesIListWithInterface()
        {
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(c => c.GetAll()).Returns(new List<ContentTypeDescriptor> { new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)), new ContentTypeDescriptor("contentTypeB", typeof(ContentTypeB)) });
            Mock.Get(contentTypeProvider).Setup(c => c.Get(typeof(ContentTypeA))).Returns(new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)));
            Mock.Get(contentTypeProvider).Setup(c => c.Get(typeof(ContentTypeB))).Returns(new ContentTypeDescriptor("contentTypeB", typeof(ContentTypeB)));

            var content = new ContentTypeB { Items = new List<InterfaceA> { new ContentTypeA() } };
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = {
                    new ContentJsonConverter<ContentTypeA>(contentTypeProvider),
                    new ContentJsonConverter<InterfaceA>(contentTypeProvider),
                    new ContentJsonConverter<ContentTypeB>(contentTypeProvider),
                }
            };

            var result = JsonSerializer.Serialize(content, options);

            Assert.Equal("{\"Type\":\"contentTypeB\",\"Value\":{\"Items\":[{\"Type\":\"contentTypeA\",\"Value\":{\"Test\":\"Lorem\"}}]}}", result);
        }

        [Fact]
        public void DeserializesIListWithInterface()
        {
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(c => c.GetAll()).Returns(new List<ContentTypeDescriptor> { new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)), new ContentTypeDescriptor("contentTypeB", typeof(ContentTypeB)) });
            Mock.Get(contentTypeProvider).Setup(c => c.Get("contentTypeA")).Returns(new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)));
            Mock.Get(contentTypeProvider).Setup(c => c.Get("contentTypeB")).Returns(new ContentTypeDescriptor("contentTypeB", typeof(ContentTypeB)));

            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = {
                    new ContentJsonConverter<ContentTypeA>(contentTypeProvider),
                    new ContentJsonConverter<InterfaceA>(contentTypeProvider),
                    new ContentJsonConverter<ContentTypeB>(contentTypeProvider),
                }
            };

            var value = "{\"Type\":\"contentTypeB\",\"Value\":{\"Items\":[{\"Type\":\"contentTypeA\",\"Value\":{\"Test\":\"Lorem\"}}]}}";
            var result = JsonSerializer.Deserialize<ContentTypeB>(value, options);

            Assert.Single(result.Items);
            Assert.IsType<ContentTypeA>(result.Items.Single());
            Assert.Equal("Lorem", ((ContentTypeA)result.Items.Single()).Test);
        }

        [Fact]
        public void DeserializesEmptyIListWithInterface()
        {
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(c => c.GetAll()).Returns(new List<ContentTypeDescriptor> { new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)) });
            Mock.Get(contentTypeProvider).Setup(c => c.Get("contentTypeA")).Returns(new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)));

            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = {
                    new ContentJsonConverter<ContentTypeA>(contentTypeProvider),
                }
            };

            var value = "{\"Type\":\"contentTypeB\",\"Value\":{\"Items\":[]}}";
            var result = JsonSerializer.Deserialize<ContentTypeB>(value, options);

            Assert.Empty(result.Items);
        }

        [Fact]
        public void DeserializesContent()
        {
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(c => c.GetAll()).Returns(new List<ContentTypeDescriptor> { new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)) });
            Mock.Get(contentTypeProvider).Setup(c => c.Get("contentTypeA")).Returns(new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)));

            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = {
                    new ContentJsonConverter<ContentTypeA>(contentTypeProvider),
                }
            };

            var value = "{\"Type\":\"contentTypeA\",\"Value\":{\"Test\":\"Lorem\"}}";
            var result = JsonSerializer.Deserialize<ContentTypeA>(value, options);

            Assert.Equal("Lorem", result.Test);
        }

        [Fact]
        public void DeserializesInterface()
        {
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(c => c.GetAll()).Returns(new List<ContentTypeDescriptor> { new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)) });
            Mock.Get(contentTypeProvider).Setup(c => c.Get("contentTypeA")).Returns(new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)));

            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = {
                    new ContentJsonConverter<InterfaceA>(contentTypeProvider),
                }
            };

            var value = "{\"Type\":\"contentTypeA\",\"Value\":{}}";
            var result = JsonSerializer.Deserialize<InterfaceA>(value, options);

            Assert.IsType<ContentTypeA>(result);
        }
    }
}
