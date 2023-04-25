using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.UI.FieldSupport;
using Cloudy.CMS.UI.FormSupport;
using Cloudy.CMS.UI.FormSupport.ChangeHandlers;
using Cloudy.CMS.UI.FormSupport.Changes;
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
    public class SimpleChangeHandlerTests
    {
        [Fact]
        public void SimpleChange()
        {
            var entity = new Entity();
            var value = "Lorem";
            var change = new SimpleChange { Path = new string[] { nameof(Entity.SimpleProperty) }, Value = JsonSerializer.SerializeToElement(value) };

            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(e => e.Get(typeof(Entity))).Returns(new EntityTypeDescriptor(nameof(Entity), typeof(Entity)));

            var fieldProvider = Mock.Of<IFieldProvider>();
            Mock.Get(fieldProvider).Setup(f => f.Get(nameof(Entity))).Returns(new List<FieldDescriptor> {
                new FieldDescriptor(nameof(Entity.SimpleProperty), typeof(string)),
            });

            new SimpleChangeHandler(entityTypeProvider, fieldProvider, Mock.Of<IEntityNavigator>()).SetValue(entity, change);

            Assert.Equal(value, entity.SimpleProperty);
        }

        public class Entity
        {
            public string SimpleProperty { get; set; }
        }
    }
}
