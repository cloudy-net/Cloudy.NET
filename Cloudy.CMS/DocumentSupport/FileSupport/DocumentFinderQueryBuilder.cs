using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class DocumentFinderQueryBuilder : IDocumentFinderQueryBuilder
    {
        IFilePathProvider FilePathProvider { get; }
        IFileHandler FileHandler { get; }
        IDocumentDeserializer DocumentDeserializer { get; }
        IDocumentPropertyPathProvider DocumentPropertyPathProvider { get; }
        IDocumentPropertyFinder DocumentPropertyFinder { get; }

        public DocumentFinderQueryBuilder(IFilePathProvider filePathProvider, IFileHandler fileHandler, IDocumentPropertyPathProvider documentPropertyPathProvider, IDocumentDeserializer documentDeserializer, IDocumentPropertyFinder documentPropertyFinder)
        {
            FilePathProvider = filePathProvider;
            FileHandler = fileHandler;
            DocumentDeserializer = documentDeserializer;
            DocumentPropertyPathProvider = documentPropertyPathProvider;
            DocumentPropertyFinder = documentPropertyFinder;
        }

        public string Container { get; set; }

        List<Func<Document, bool>> Criteria { get; } = new List<Func<Document, bool>>();

        public IDocumentFinderQueryBuilder WhereEquals<T1, T2>(Expression<Func<T1, T2>> property, T2 value) where T1 : class
        {
            var path = DocumentPropertyPathProvider.GetFor(property);

            Criteria.Add(d =>
            {
                var a = DocumentPropertyFinder.GetFor(d, path);
                
                if(a == null)
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
            var path = DocumentPropertyPathProvider.GetFor(property);

            Criteria.Add(d => DocumentPropertyFinder.Exists(d, path));

            return this;
        }

        public IDocumentFinderQueryBuilder WhereIn<T1, T2>(Expression<Func<T1, T2>> property, IEnumerable<T2> values) where T1 : class
        {
            var path = DocumentPropertyPathProvider.GetFor(property);

            Criteria.Add(d =>
            {
                var a = DocumentPropertyFinder.GetFor(d, path);

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

            foreach(var document in FileHandler.List(FilePathProvider.GetPathFor(Container)).Select(c => DocumentDeserializer.Deserialize(c)))
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