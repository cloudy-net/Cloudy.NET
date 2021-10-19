using System;
using System.Runtime.Serialization;

namespace Cloudy.CMS.ContentSupport
{
    public class CouldNotFindAnyDbSetForTypeInsideContextException : Exception
    {
        public CouldNotFindAnyDbSetForTypeInsideContextException(Type type, Type contextType) : base($"Could not find a DbSet with a type compatible with (or assignable to) {type} inside DbContext {contextType}") { }
    }
}