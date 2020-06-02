using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class DocumentFinder : IDocumentFinder
    {
        IServiceProvider ServiceProvider { get; }

        public DocumentFinder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IDocumentFinderQueryBuilder Find(string container)
        {
            var builder = ((IDocumentFinderQueryBuilder)ServiceProvider.GetService(typeof(IDocumentFinderQueryBuilder)));
            
            builder.Container = container;

            return builder;
        }
    }
}
