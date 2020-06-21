using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public class ContentRouteCreator : IContentRouteCreator
    {
        ILogger Logger { get; }
        EndpointDataSource EndpointDataSource { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContentTypeExpander ContentTypeExpander { get; }

        public ContentRouteCreator(ILogger<ContentRouteCreator> logger, EndpointDataSource endpointDataSource, IContentTypeProvider contentTypeProvider, IContentTypeExpander contentTypeExpander)
        {
            Logger = logger;
            EndpointDataSource = endpointDataSource;
            ContentTypeProvider = contentTypeProvider;
            ContentTypeExpander = contentTypeExpander;
        }

        public IEnumerable<ContentRouteDescriptor> Create()
        {
            var endpointDataSource = EndpointDataSource as CompositeEndpointDataSource;

            if (endpointDataSource == null)
            {
                return Enumerable.Empty<ContentRouteDescriptor>();
            }

            var result = new List<ContentRouteDescriptor>();

            foreach(var dataSource in endpointDataSource.DataSources)
            {
                foreach(var endpoint in dataSource.Endpoints)
                {
                    var routeEndpoint = endpoint as RouteEndpoint;

                    if(routeEndpoint == null)
                    {
                        continue;
                    }

                    if (routeEndpoint.RoutePattern.PathSegments.Any(s => s.Parts.Count == 0 || s.Parts.Count > 1 || !s.IsSimple)) // ignore complex segments (combining several parts between slashes)
                    {
                        continue;
                    }

                    if (routeEndpoint.RoutePattern.Parameters.OfType<RoutePatternParameterPart>().SelectMany(s => s.ParameterPolicies).Count(p => p.Content == "contentroute" || p.Content.StartsWith("contentroute(")) != 1) // ignore non-content routes AND multi-content routes
                    {
                        continue;
                    }

                    if (routeEndpoint.RoutePattern.Parameters.OfType<RoutePatternParameterPart>().Any(s => s.ParameterPolicies.Any(p => p.Content != "contentroute" && !p.Content.StartsWith("contentroute(")))) // ignore routes with non-content parameters
                    {
                        continue;
                    }

                    IEnumerable<ContentTypeDescriptor> contentTypes = null;

                    var path = new List<string>();

                    foreach(var segment in routeEndpoint.RoutePattern.PathSegments)
                    {
                        var part = segment.Parts.Single();

                        var literal = part as RoutePatternLiteralPart;
                        if (literal != null)
                        {
                            path.Add(literal.Content);
                        }

                        var parameter = part as RoutePatternParameterPart;
                        if (parameter != null)
                        {
                            var content = parameter.ParameterPolicies[0].Content;

                            var startParenthesisIndex = content.IndexOf('(');
                            var endParenthesisIndex = content.IndexOf(')');
                            var name = startParenthesisIndex == -1 ? content : content.Substring(0, startParenthesisIndex);

                            var contentTypeOrGroupIdOrTypeName = startParenthesisIndex == -1 ? null : content.Substring(startParenthesisIndex + 1, endParenthesisIndex - (startParenthesisIndex + 1));

                            if(contentTypeOrGroupIdOrTypeName != null)
                            {
                                contentTypes = ContentTypeExpander.Expand(contentTypeOrGroupIdOrTypeName).ToList().AsReadOnly();
                            }

                            path.Add($"{{{name}}}");
                        }
                    }

                    if(contentTypes == null)
                    {
                        contentTypes = ContentTypeProvider.GetAll().ToList().AsReadOnly();
                    }

                    result.Add(new ContentRouteDescriptor(string.Join("/", path), contentTypes));
                }
            }

            return result.AsReadOnly();

            //var test = .RoutePattern.Parameters[0].ParameterPolicies.Any(p => p.Content == "contentroute" || p.Content.StartsWith("contentroute("));

        }
    }
}
