using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Cloudy.CMS.DocumentSupport
{
    public class DocumentPropertyPathProvider : IDocumentPropertyPathProvider
    {
        ICoreInterfaceProvider CoreInterfaceProvider { get; }

        public DocumentPropertyPathProvider(ICoreInterfaceProvider coreInterfaceProvider)
        {
            CoreInterfaceProvider = coreInterfaceProvider;
        }

        public string GetFor(Expression expression)
        {
            var lambda = (LambdaExpression)expression;

            if(lambda.Parameters.Count != 1)
            {
                throw new OnlyOneParameterAllowedInExpressionException(lambda.Parameters);
            }

            var coreInterface = CoreInterfaceProvider.GetFor(lambda.Parameters.Single().Type);

            var segments = new List<string>();

            segments.Add(nameof(Document.GlobalFacet));
            segments.Add(nameof(DocumentFacet.Interfaces));
            segments.Add(coreInterface.Id);
            segments.Add(nameof(DocumentInterface.Properties));

            var memberExpression = lambda.Body as MemberExpression;

            if (memberExpression == null)
            {
                var body = (UnaryExpression)lambda.Body;
                memberExpression = body.Operand as MemberExpression;
            }

            segments.Add(memberExpression.Member.Name);

            return string.Join(".", segments);
        }
    }
}
