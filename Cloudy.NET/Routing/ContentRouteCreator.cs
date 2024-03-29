﻿using Cloudy.NET.EntityTypeSupport;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEntityTypeProvider = Cloudy.NET.EntityTypeSupport.IEntityTypeProvider;

namespace Cloudy.NET.Routing
{
    public class ContentRouteCreator : IContentRouteCreator
    {
        ILogger Logger { get; }
        EndpointDataSource EndpointDataSource { get; }
        IEntityTypeProvider EntityTypeProvider { get; }

        public ContentRouteCreator(ILogger<ContentRouteCreator> logger, EndpointDataSource endpointDataSource, IEntityTypeProvider entityTypeProvider)
        {
            Logger = logger;
            EndpointDataSource = endpointDataSource;
            EntityTypeProvider = entityTypeProvider;
        }

        public IEnumerable<ContentRouteDescriptor> Create()
        {
            var endpointDataSource = EndpointDataSource as CompositeEndpointDataSource;

            if (endpointDataSource == null)
            {
                return Enumerable.Empty<ContentRouteDescriptor>();
            }

            var result = new Dictionary<string, ContentRouteDescriptor>();

            foreach (var endpoint in endpointDataSource.Endpoints)
            {
                var routeEndpoint = endpoint as RouteEndpoint;

                if (routeEndpoint == null)
                {
                    continue;
                }

                if (result.ContainsKey(routeEndpoint.RoutePattern.RawText))
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

                IEnumerable<Type> types = null;

                var path = new List<string>();

                foreach (var segment in routeEndpoint.RoutePattern.PathSegments)
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

                        var typeName = startParenthesisIndex == -1 ? null : content.Substring(startParenthesisIndex + 1, endParenthesisIndex - (startParenthesisIndex + 1));

                        if (typeName != null)
                        {
                            var entityType = EntityTypeProvider.Get(typeName);

                            if (entityType == null)
                            {
                                throw new Exception($"Could not find content type with name {typeName} for content route");
                            }

                            types = new List<Type> { entityType.Type }.AsReadOnly();
                        }

                        path.Add($"{{{name}}}");
                    }
                }

                if (types == null)
                {
                    types = EntityTypeProvider.GetAll().Select(t => t.Type).ToList().AsReadOnly();
                }

                result[routeEndpoint.RoutePattern.RawText] = new ContentRouteDescriptor(string.Join("/", path), types);
            }

            return result.Values.ToList().AsReadOnly();
        }
    }
}
