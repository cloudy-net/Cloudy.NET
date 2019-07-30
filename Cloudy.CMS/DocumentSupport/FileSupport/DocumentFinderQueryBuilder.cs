using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class DocumentFinderQueryBuilder : IDocumentFinderQueryBuilder
    {
        IFileHandler FileHandler { get; }
        IDocumentDeserializer DocumentDeserializer { get; }
        IPropertyPathProvider PropertyPathProvider { get; }
        IDocumentPropertyProvider DocumentPropertyProvider { get; }

        public DocumentFinderQueryBuilder(IFileHandler fileHandler, IPropertyPathProvider propertyPathProvider, IDocumentDeserializer documentDeserializer, IDocumentPropertyProvider documentPropertyProvider)
        {
            FileHandler = fileHandler;
            DocumentDeserializer = documentDeserializer;
            PropertyPathProvider = propertyPathProvider;
            DocumentPropertyProvider = documentPropertyProvider;
        }

        public string Container { get; set; }

        List<Func<Document, bool>> Criteria { get; } = new List<Func<Document, bool>>();
        List<Func<>>

        public IDocumentFinderQueryBuilder WhereEquals<T1, T2>(Expression<Func<T1, T2>> property, T2 value) where T1 : class
        {
            var path = PropertyPathProvider.GetFor(property);

            Criteria.Add(d =>
            {
                var a = DocumentPropertyProvider.GetFor(d, path);
                
                if(a == null && value == null)
                {
                    if (value == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return a.Equals(value);
            });

            return this;
        }

        public IDocumentFinderQueryBuilder WhereExists<T1, T2>(Expression<Func<T1, T2>> property) where T1 : class
        {
            var path = PropertyPathProvider.GetFor(property);

            Criteria.Add(d => DocumentPropertyProvider.Exists(d, path));

            return this;
        }

        public IDocumentFinderQueryBuilder WhereIn<T1, T2>(Expression<Func<T1, T2>> property, IEnumerable<T2> values) where T1 : class
        {
            var path = PropertyPathProvider.GetFor(property);

            Criteria.Add(d =>
            {
                var a = DocumentPropertyProvider.GetFor(d, path);

                if (a == null)
                {
                    return false;
                }

                if(!(a is T2))
                {
                    return false;
                }

                return values.Contains((T2)a);
            });

            return this;
        }

        public IDocumentFinderQueryBuilder Select<T1, T2>(Expression<Func<T1, T2>> property) where T1 : class
        {
            return this;
        }

        public Task<IEnumerable<Document>> GetResultAsync()
        {
            var result = new List<Document>();

            foreach(var document in FileHandler.List(Container).Select(c => DocumentDeserializer.Deserialize(c)))
            {
                if(Criteria.All(c => c(document)))
                {
                    result.Add(document);
                }
            }

            return Task.FromResult((IEnumerable<Document>)result);
        }
    }
}