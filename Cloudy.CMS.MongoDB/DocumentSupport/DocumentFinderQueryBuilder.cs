using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class DocumentFinderQueryBuilder : IDocumentFinderQueryBuilder
    {
        IDocumentPropertyPathProvider DocumentPropertyPathProvider { get; }
        IContainerProvider ContainerProvider { get; }

        public DocumentFinderQueryBuilder(IDocumentPropertyPathProvider documentPropertyPathProvider, IContainerProvider containerProvider)
        {
            DocumentPropertyPathProvider = documentPropertyPathProvider;
            ContainerProvider = containerProvider;
        }

        public string Container { get; set; }
        List<FilterDefinition<Document>> Filters { get; } = new List<FilterDefinition<Document>>();
        ProjectionDefinition<Document> Projection { get; set; }

        public IDocumentFinderQueryBuilder Select<T1, T2>(Expression<Func<T1, T2>> property) where T1 : class
        {
            if(Projection == null)
            {
                Projection = Builders<Document>.Projection.Include(new StringFieldDefinition<Document, T2>(DocumentPropertyPathProvider.GetFor(property)));
            }
            else
            {
                Projection = Projection.Include(new StringFieldDefinition<Document, T2>(DocumentPropertyPathProvider.GetFor(property)));
            }

            return this;
        }

        public IDocumentFinderQueryBuilder WhereEquals<T1, T2>(Expression<Func<T1, T2>> property, T2 value) where T1 : class
        {
            if(value == null)
            {
                Filters.Add(
                    Builders<Document>.Filter.And(
                        Builders<Document>.Filter.Exists(new StringFieldDefinition<Document, T2>(DocumentPropertyPathProvider.GetFor(property))),
                        Builders<Document>.Filter.Eq(new StringFieldDefinition<Document, T2>(DocumentPropertyPathProvider.GetFor(property)), value)
                    )
                );
            }
            else
            {
                Filters.Add(
                    Builders<Document>.Filter.Eq(new StringFieldDefinition<Document, T2>(DocumentPropertyPathProvider.GetFor(property)), value)
                );
            }

            return this;
        }

        public IDocumentFinderQueryBuilder WhereIn<T1, T2>(Expression<Func<T1, T2>> property, IEnumerable<T2> values) where T1 : class
        {
            Filters.Add(Builders<Document>.Filter.In(new StringFieldDefinition<Document, T2>(DocumentPropertyPathProvider.GetFor(property)), values));

            return this;
        }

        public IDocumentFinderQueryBuilder WhereExists<T1, T2>(Expression<Func<T1, T2>> property) where T1 : class
        {
            Filters.Add(Builders<Document>.Filter.Exists(new StringFieldDefinition<Document, T2>(DocumentPropertyPathProvider.GetFor(property))));

            return this;
        }

        public async Task<IEnumerable<Document>> GetResultAsync()
        {
            var options = new FindOptions<Document, Document>();

            if(Projection != null)
            {
                options.Projection = Projection;
            }

            var cursor = await ContainerProvider.Get(Container).FindAsync(Builders<Document>.Filter.And(Filters), options);

            return await cursor.ToListAsync();
        }
    }
}
