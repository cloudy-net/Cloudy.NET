using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.UI.FieldSupport;
using Cloudy.NET.UI.FormSupport;
using Cloudy.NET.UI.FormSupport.ChangeHandlers;
using Cloudy.NET.UI.FormSupport.Changes;
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
    public class BlockTypeChangeHandlerTests
    {
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

            new BlockTypeChangeHandler(entityTypeProvider, fieldProvider).SetType(entity, change);

            Assert.IsType<Implementation>(entity.InterfaceProperty);
        }

        public class Entity
        {
            public IInterface InterfaceProperty { get; set; }
        }

        public interface IInterface { }

        public class Implementation : IInterface { }
    }
}
