using Cloudy.CMS.InitializerSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.EntitySupport.Serialization
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
