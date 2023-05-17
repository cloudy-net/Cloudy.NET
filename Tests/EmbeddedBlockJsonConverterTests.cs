using Cloudy.NET.EntitySupport.Serialization;
using Cloudy.NET.EntityTypeSupport;
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
    public class EmbeddedBlockJsonConverterTests
    {
        public class EntityTypeA : InterfaceA
        {
            public string Test { get; set; } = "Lorem";
        }

        public class EntityTypeB
        {
            public IList<InterfaceA> Items { get; set; }
        }

        public class InterfaceA
        {
        }

        [Fact]
        public void SerializesIListWithInterface()
        {
            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(c => c.GetAll()).Returns(new List<EntityTypeDescriptor> { new EntityTypeDescriptor("entityTypeA", typeof(EntityTypeA)), new EntityTypeDescriptor("entityTypeB", typeof(EntityTypeB)) });
            Mock.Get(entityTypeProvider).Setup(c => c.Get(typeof(EntityTypeA))).Returns(new EntityTypeDescriptor("entityTypeA", typeof(EntityTypeA)));
            Mock.Get(entityTypeProvider).Setup(c => c.Get(typeof(EntityTypeB))).Returns(new EntityTypeDescriptor("entityTypeB", typeof(EntityTypeB)));

            var content = new EntityTypeB { Items = new List<InterfaceA> { new EntityTypeA() } };
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = {
                    new EmbeddedBlockJsonConverter<EntityTypeA>(entityTypeProvider),
                    new EmbeddedBlockJsonConverter<InterfaceA>(entityTypeProvider),
                    new EmbeddedBlockJsonConverter<EntityTypeB>(entityTypeProvider),
                }
            };

            var result = JsonSerializer.Serialize(content, options);

            Assert.Equal("{\"Type\":\"entityTypeB\",\"Value\":{\"Items\":[{\"Type\":\"entityTypeA\",\"Value\":{\"Test\":\"Lorem\"}}]}}", result);
        }

        [Fact]
        public void DeserializesIListWithInterface()
        {
            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(c => c.GetAll()).Returns(new List<EntityTypeDescriptor> { new EntityTypeDescriptor("entityTypeA", typeof(EntityTypeA)), new EntityTypeDescriptor("entityTypeB", typeof(EntityTypeB)) });
            Mock.Get(entityTypeProvider).Setup(c => c.Get("entityTypeA")).Returns(new EntityTypeDescriptor("entityTypeA", typeof(EntityTypeA)));
            Mock.Get(entityTypeProvider).Setup(c => c.Get("entityTypeB")).Returns(new EntityTypeDescriptor("entityTypeB", typeof(EntityTypeB)));

            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = {
                    new EmbeddedBlockJsonConverter<EntityTypeA>(entityTypeProvider),
                    new EmbeddedBlockJsonConverter<InterfaceA>(entityTypeProvider),
                    new EmbeddedBlockJsonConverter<EntityTypeB>(entityTypeProvider),
                }
            };

            var value = "{\"Type\":\"entityTypeB\",\"Value\":{\"Items\":[{\"Type\":\"entityTypeA\",\"Value\":{\"Test\":\"Lorem\"}}]}}";
            var result = JsonSerializer.Deserialize<EntityTypeB>(value, options);

            Assert.Single(result.Items);
            Assert.IsType<EntityTypeA>(result.Items.Single());
            Assert.Equal("Lorem", ((EntityTypeA)result.Items.Single()).Test);
        }

        [Fact]
        public void DeserializesEmptyIListWithInterface()
        {
            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(c => c.GetAll()).Returns(new List<EntityTypeDescriptor> { new EntityTypeDescriptor("entityTypeB", typeof(EntityTypeB)) });
            Mock.Get(entityTypeProvider).Setup(c => c.Get("entityTypeB")).Returns(new EntityTypeDescriptor("entityTypeB", typeof(EntityTypeB)));

            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = {
                    new EmbeddedBlockJsonConverter<EntityTypeB>(entityTypeProvider),
                }
            };

            var value = "{\"Type\":\"entityTypeB\",\"Value\":{\"Items\":[]}}";
            var result = JsonSerializer.Deserialize<EntityTypeB>(value, options);

            Assert.Empty(result.Items);
        }

        [Fact]
        public void DeserializesContent()
        {
            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(c => c.GetAll()).Returns(new List<EntityTypeDescriptor> { new EntityTypeDescriptor("entityTypeA", typeof(EntityTypeA)) });
            Mock.Get(entityTypeProvider).Setup(c => c.Get("entityTypeA")).Returns(new EntityTypeDescriptor("entityTypeA", typeof(EntityTypeA)));

            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = {
                    new EmbeddedBlockJsonConverter<EntityTypeA>(entityTypeProvider),
                }
            };

            var value = "{\"Type\":\"entityTypeA\",\"Value\":{\"Test\":\"Lorem\"}}";
            var result = JsonSerializer.Deserialize<EntityTypeA>(value, options);

            Assert.Equal("Lorem", result.Test);
        }

        [Fact]
        public void DeserializesInterface()
        {
            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(c => c.GetAll()).Returns(new List<EntityTypeDescriptor> { new EntityTypeDescriptor("entityTypeA", typeof(EntityTypeA)) });
            Mock.Get(entityTypeProvider).Setup(c => c.Get("entityTypeA")).Returns(new EntityTypeDescriptor("entityTypeA", typeof(EntityTypeA)));

            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = {
                    new EmbeddedBlockJsonConverter<InterfaceA>(entityTypeProvider),
                }
            };

            var value = "{\"Type\":\"entityTypeA\",\"Value\":{}}";
            var result = JsonSerializer.Deserialize<InterfaceA>(value, options);

            Assert.IsType<EntityTypeA>(result);
        }
    }
}
