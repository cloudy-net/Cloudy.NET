using System;

namespace Cloudy.CMS.DocumentSupport.FileSupport
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
            var builder = (DocumentFinderQueryBuilder)ServiceProvider.GetService(typeof(IDocumentFinderQueryBuilder));
            
            builder.Container = container;

            return builder;
        }
    }
}