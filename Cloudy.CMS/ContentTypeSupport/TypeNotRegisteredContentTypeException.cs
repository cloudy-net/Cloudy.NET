using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class TypeNotRegisteredContentTypeException : Exception
    {
        public TypeNotRegisteredContentTypeException(Type type) : base($"This content has no content type (or rather the type `{type}` has no [ContentType] attribute), or its type is not a known assembly to Cloudy, in which you need to do .AddAssembly<SomeTypeFromAssembly>() at startup") { }
    }
}
