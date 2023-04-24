using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.UI.FieldSupport;
using Cloudy.CMS.UI.FormSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class EntityChangeApplierTests
    {
        [Fact]
        public void SimpleChange()
        {
            var entity = new Entity();
            var value = "Lorem";
            var change = new SimpleChange { Path = new string[] { nameof(Entity.SimpleProperty) }, Value = JsonSerializer.Serialize(value) };

            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(e => e.Get(typeof(Entity))).Returns(new EntityTypeDescriptor(nameof(Entity), typeof(Entity)));

            var fieldProvider = Mock.Of<IFieldProvider>();
            Mock.Get(fieldProvider).Setup(f => f.Get(nameof(Entity))).Returns(new List<FieldDescriptor> {
                new FieldDescriptor(nameof(Entity.SimpleProperty), typeof(string)),
            });

            new EntityChangeApplier(entityTypeProvider, fieldProvider).Apply(entity, change, Mock.Of<IListTracker>());

            Assert.Equal(value, entity.SimpleProperty);
        }

        [Fact]
        public void BlockTypeChange()
        {
            var entity = new Entity();
            var change = new BlockTypeChange { Path = new string[] { nameof(Entity.InterfaceProperty) }, Type = nameof(Implementation) };

            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(e => e.Get(typeof(Entity))).Returns(new EntityTypeDescriptor(nameof(Entity), typeof(Entity)));
            Mock.Get(entityTypeProvider).Setup(e => e.Get(nameof(Implementation))).Returns(new EntityTypeDescriptor(nameof(Implementation), typeof(Implementation)));

            var fieldProvider = Mock.Of<IFieldProvider>();
            Mock.Get(fieldProvider).Setup(f => f.Get(nameof(Entity))).Returns(new List<FieldDescriptor> {
                new FieldDescriptor(nameof(Entity.InterfaceProperty), typeof(IInterface)),
            });

            new EntityChangeApplier(entityTypeProvider, fieldProvider).Apply(entity, change, Mock.Of<IListTracker>());

            Assert.IsType<Implementation>(entity.InterfaceProperty);
        }

        [Fact]
        public void AddToEmbeddedBlockList()
        {
            var entity = new Entity();
            var change = new EmbeddedBlockListAdd { Path = new string[] { nameof(Entity.EmbeddedBlockList) }, Type = nameof(Implementation) };

            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(e => e.Get(typeof(Entity))).Returns(new EntityTypeDescriptor(nameof(Entity), typeof(Entity)));
            Mock.Get(entityTypeProvider).Setup(e => e.Get(nameof(Implementation))).Returns(new EntityTypeDescriptor(nameof(Implementation), typeof(Implementation)));

            var fieldProvider = Mock.Of<IFieldProvider>();
            Mock.Get(fieldProvider).Setup(f => f.Get(nameof(Entity))).Returns(new List<FieldDescriptor> {
                new FieldDescriptor(nameof(Entity.EmbeddedBlockList), typeof(IInterface)),
            });

            new EntityChangeApplier(entityTypeProvider, fieldProvider).Apply(entity, change, Mock.Of<IListTracker>());

            Assert.IsType<Implementation>(entity.EmbeddedBlockList.Single());
        }

        public class Entity
        {
            public string SimpleProperty { get; set; }
            public IInterface InterfaceProperty { get; set; }
            public IList<IInterface> EmbeddedBlockList { get; set; }
        }

        public interface IInterface { }

        public class Implementation : IInterface { }
    }
}
