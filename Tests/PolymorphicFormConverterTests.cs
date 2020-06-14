using Cloudy.CMS.UI.ContentAppSupport.Controllers;
using Cloudy.CMS.UI.FormSupport;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class PolymorphicFormConverterTests
    {
        [Fact]
        public void Deserializes()
        {
            var form = new FormDescriptor("lorem", typeof(FormA));
            var formProvider = Mock.Of<IFormProvider>();
            Mock.Get(formProvider).Setup(p => p.GetAll()).Returns(new List<FormDescriptor> { form });
            Mock.Get(formProvider).Setup(p => p.Get("lorem")).Returns(form);

            var sut = new PolymorphicFormConverter(Mock.Of<ILogger<PolymorphicFormConverter>>(), formProvider);
            var result = JsonConvert.DeserializeObject<ContentTypeA>("{ form: { formId: 'lorem', value: { property: 'ipsum' } } }", sut);

            Assert.IsType<FormA>(result.Form);
            Assert.Equal("ipsum", ((FormA)result.Form).Property);
        }

        [Fact]
        public void Serializes()
        {
            var form = new FormDescriptor("lorem", typeof(FormA));
            var formProvider = Mock.Of<IFormProvider>();
            Mock.Get(formProvider).Setup(p => p.GetAll()).Returns(new List<FormDescriptor> { form });
            Mock.Get(formProvider).Setup(p => p.Get("lorem")).Returns(form);
            Mock.Get(formProvider).Setup(p => p.Get(typeof(FormA))).Returns(form);

            var sut = new PolymorphicFormConverter(Mock.Of<ILogger<PolymorphicFormConverter>>(), formProvider);
            var result = JsonConvert.SerializeObject(new ContentTypeA { Form = new FormA { Property = "ipsum" } }, sut);

            Assert.Equal("{\"Form\":{\"FormId\":\"lorem\",\"Value\":{\"Property\":\"ipsum\"}}}", result);
        }

        class ContentTypeA
        {
            public InterfaceA Form { get; set; }
        }

        class FormA : InterfaceA
        {
            public string Property { get; set; }
        }

        interface InterfaceA
        {

        }
    }
}
