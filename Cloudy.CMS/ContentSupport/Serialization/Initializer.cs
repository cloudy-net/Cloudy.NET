using Cloudy.CMS.InitializerSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class Initializer : IInitializer
    {
        public Initializer(IPolymorphicSerializer polymorphicSerializer, IPolymorphicDeserializer polymorphicDeserializer)
        {
            PolymorphicSerializer.UglyInstance = polymorphicSerializer;
            PolymorphicDeserializer.UglyInstance = polymorphicDeserializer;
        }

        public async Task InitializeAsync() { }
    }
}
