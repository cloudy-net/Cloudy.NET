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
    public class FormEntityUpdaterTests
    {
        [Fact]
        public void SimpleChange()
        {
            var entity = new MyEntity();
            var value = "Lorem";
            var change = new SimpleChange { Path = new string[] { nameof(MyEntity.SimpleProperty) }, Value = JsonSerializer.Serialize(value) };
            
            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(e => e.Get(typeof(MyEntity))).Returns(new EntityTypeDescriptor(nameof(MyEntity), typeof(MyEntity)));

            var fieldProvider = Mock.Of<IFieldProvider>();
            Mock.Get(fieldProvider).Setup(f => f.Get(nameof(MyEntity))).Returns(new List<FieldDescriptor> {
                new FieldDescriptor(nameof(MyEntity.SimpleProperty), typeof(string), null, null, null, null, null, false, null, null, null),
            });

            new FormEntityUpdater(entityTypeProvider, fieldProvider).Update(entity, new List<EntityChange> { change });

            Assert.Equal(value, entity.SimpleProperty);
        }

        public class MyEntity
        {
            public string SimpleProperty { get; set; }
        }
    }
}
