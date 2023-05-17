using Cloudy.NET.EntitySupport.Serialization;
using Cloudy.NET.EntityTypeSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class EmbeddedBlockJsonConverterTypeProviderTests
    {
        abstract class BaseClassA
        {

        }
        class EntityTypeA : BaseClassA
        {

        }
        interface InterfaceB
        {

        }
        class EntityTypeB : InterfaceB
        {

        }
        class EntityTypeC : InterfaceB
        {

        }

        [Fact]
        public void AddsBaseTypes()
        {
            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(p => p.GetAll()).Returns(new List<EntityTypeDescriptor> { new EntityTypeDescriptor("entityTypeA", typeof(EntityTypeA)) });

            var result = new EmbeddedBlockJsonConverterTypeProvider(entityTypeProvider).GetAll();

            Assert.Equal(new List<Type> { typeof(EntityTypeA), typeof(BaseClassA) }.AsReadOnly(), result);
        }

        [Fact]
        public void AddsInterfaces()
        {
            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(p => p.GetAll()).Returns(new List<EntityTypeDescriptor> { new EntityTypeDescriptor("entityTypeB", typeof(EntityTypeB)) });

            var result = new EmbeddedBlockJsonConverterTypeProvider(entityTypeProvider).GetAll();

            Assert.Equal(new List<Type> { typeof(EntityTypeB), typeof(InterfaceB) }.AsReadOnly(), result);
        }

        [Fact]
        public void HandlesDuplicateTypes()
        {
            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(p => p.GetAll()).Returns(new List<EntityTypeDescriptor> { new EntityTypeDescriptor("entityTypeB", typeof(EntityTypeB)), new EntityTypeDescriptor("entityTypeC", typeof(EntityTypeC)) });

            var result = new EmbeddedBlockJsonConverterTypeProvider(entityTypeProvider).GetAll();

            Assert.Equal(new List<Type> { typeof(EntityTypeB), typeof(InterfaceB), typeof(EntityTypeC) }.AsReadOnly(), result);
        }
    }
}
