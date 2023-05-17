using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.UI.FieldSupport;
using Cloudy.NET.UI.FormSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class EntityNavigatorTests
    {
        [Fact]
        public void NavigatesToSelfIfSingleSegmentPath()
        {
            var value = "Lorem";
            object entity = new Entity
            {
                SimpleProperty = value,
            };
            var path = new string[] { nameof(Entity.SimpleProperty) };

            object expectedEntity = entity;

            entity = new EntityNavigator(Mock.Of<IEntityTypeProvider>(), Mock.Of<IFieldProvider>()).Navigate(entity, path, Mock.Of<IListTracker>());

            Assert.Equal(expectedEntity, entity);
        }

        [Fact]
        public void NavigatesEmbeddedBlock()
        {
            var value = "Lorem";
            object entity = new Entity
            {
                NestedProperty = new EmbeddedBlock
                {
                    Property = value
                }
            };
            var path = new string[] { nameof(Entity.NestedProperty), nameof(EmbeddedBlock.Property) };

            object expectedEntity = ((Entity)entity).NestedProperty;

            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(e => e.Get(typeof(Entity))).Returns(new EntityTypeDescriptor(nameof(Entity), typeof(Entity)));

            var fieldProvider = Mock.Of<IFieldProvider>();
            Mock.Get(fieldProvider).Setup(f => f.Get(nameof(Entity))).Returns(new List<FieldDescriptor> {
                new FieldDescriptor(nameof(Entity.SimpleProperty), typeof(string)),
                new FieldDescriptor(nameof(Entity.NestedProperty), typeof(EmbeddedBlock)),
            });

            entity = new EntityNavigator(entityTypeProvider, fieldProvider).Navigate(entity, path, Mock.Of<IListTracker>());

            Assert.Equal(expectedEntity, entity);
        }

        [Fact]
        public void NavigatesList()
        {
            var value = "Lorem";
            var block = new EmbeddedBlock { Property = value };
            var list = new List<EmbeddedBlock> { block };
            object entity = new Entity { BlockList = list };
            var path = new string[] { nameof(Entity.BlockList), "0", nameof(EmbeddedBlock.Property) };

            object expectedEntity = ((Entity)entity).BlockList[0];

            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(e => e.Get(typeof(Entity))).Returns(new EntityTypeDescriptor(nameof(Entity), typeof(Entity)));

            var fieldProvider = Mock.Of<IFieldProvider>();
            Mock.Get(fieldProvider).Setup(f => f.Get(nameof(Entity))).Returns(new List<FieldDescriptor> {
                new FieldDescriptor(nameof(Entity.BlockList), typeof(EmbeddedBlock)),
            });

            var listTracker = Mock.Of<IListTracker>();
            Mock.Get(listTracker).Setup(l => l.GetElement(list, "0")).Returns(block);

            entity = new EntityNavigator(entityTypeProvider, fieldProvider).Navigate(entity, path, listTracker);

            Assert.Equal(expectedEntity, entity);
        }

        public class Entity
        {
            public string SimpleProperty { get; set; }
            public EmbeddedBlock NestedProperty { get; set; }
            public IList<EmbeddedBlock> BlockList { get; set; }
        }

        public class EmbeddedBlock
        {
            public string Property { get; set; }
        }
    }
}
