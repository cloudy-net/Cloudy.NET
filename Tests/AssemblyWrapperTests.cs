using Cloudy.CMS;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace Tests
{
    public class AssemblyWrapperTests
    {
        [Fact]
        public void EqualsIfSameAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Assert.True(new AssemblyWrapper(assembly).Equals(new AssemblyWrapper(assembly)));
        }

        [Fact]
        public void EqualsIfComparedToItsAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Assert.True(new AssemblyWrapper(assembly).Equals(assembly));
        }
    }
}
