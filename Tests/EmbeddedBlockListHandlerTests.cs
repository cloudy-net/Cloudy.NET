using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.UI.FieldSupport;
using Cloudy.CMS.UI.FormSupport.Changes;
using Cloudy.CMS.UI.FormSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Cloudy.CMS.UI.FormSupport.ChangeHandlers;

namespace Tests
{
    public class EmbeddedBlockListHandlerTests
    {
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

            new EmbeddedBlockListHandler(entityTypeProvider, fieldProvider).Add(entity, change);

            Assert.IsType<Implementation>(entity.EmbeddedBlockList.Single());
        }

        public class Entity
        {
            public IList<IInterface> EmbeddedBlockList { get; set; }
        }

        public interface IInterface { }

        public class Implementation : IInterface { }
    }
}
