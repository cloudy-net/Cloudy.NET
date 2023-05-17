using Cloudy.NET.InitializerSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.EntitySupport.Serialization
{
    public class Initializer : IInitializer
    {
        public Initializer(IEmbeddedBlockJsonConverterProvider contentJsonConverterProvider)
        {
            EmbeddedBlockJsonConverterProvider.UglyInstance = contentJsonConverterProvider;
        }

        public async Task InitializeAsync() { }
    }
}
