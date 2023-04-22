﻿using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.UI.FieldSupport;
using Cloudy.CMS.UI.FormSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class EntityPathNavigatorTests
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
            var expectedPath = new string[] { nameof(Entity.SimpleProperty) };

            new EntityPathNavigator(Mock.Of<IEntityTypeProvider>(), Mock.Of<IFieldProvider>()).Navigate(ref entity, ref path);

            Assert.Equal(expectedEntity, entity);
            Assert.Equal(expectedPath, path);
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
            var expectedPath = new string[] { nameof(EmbeddedBlock.Property) };

            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(e => e.Get(typeof(Entity))).Returns(new EntityTypeDescriptor(nameof(Entity), typeof(Entity)));

            var fieldProvider = Mock.Of<IFieldProvider>();
            Mock.Get(fieldProvider).Setup(f => f.Get(nameof(Entity))).Returns(new List<FieldDescriptor> {
                new FieldDescriptor(nameof(Entity.SimpleProperty), typeof(string)),
                new FieldDescriptor(nameof(Entity.NestedProperty), typeof(EmbeddedBlock)),
            });

            new EntityPathNavigator(entityTypeProvider, fieldProvider).Navigate(ref entity, ref path);

            Assert.Equal(expectedEntity, entity);
            Assert.Equal(expectedPath, path);
        }

        public class Entity
        {
            public string SimpleProperty { get; set; }
            public EmbeddedBlock NestedProperty { get; set; }
        }

        public class EmbeddedBlock
        {
            public string Property { get; set; }
        }
    }
}