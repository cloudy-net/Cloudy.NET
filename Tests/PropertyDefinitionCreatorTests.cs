using Cloudy.CMS.PropertyDefinitionSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class PropertyDefinitionCreatorTests
    {
        [Fact]
        public void IncludesInterfaceAttributes() {
            var result = new PropertyDefinitionCreator().Create(typeof(MyClass).GetProperty(nameof(MyClass.MyProperty)));

            Assert.Single(result.Attributes);
        }

        public class MyClass : MyInterface
        {
            public string MyProperty { get; set; }
        }

        public interface MyInterface
        {
            [Display]
            string MyProperty { get; set; }
        }
    }
}
