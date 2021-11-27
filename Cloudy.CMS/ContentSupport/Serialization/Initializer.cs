using Cloudy.CMS.InitializerSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class Initializer : IInitializer
    {
        public Initializer(IContentJsonConverterProvider contentJsonConverterProvider)
        {
            ContentJsonConverterProvider.UglyInstance = contentJsonConverterProvider;
        }

        public async Task InitializeAsync() { }
    }
}
