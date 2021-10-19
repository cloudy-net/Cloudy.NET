using System;
using System.Runtime.Serialization;

namespace Cloudy.CMS.ContentSupport
{
    public class CouldNotFindAnyDbContextWithDbSetForTypeException : Exception
    {
        public CouldNotFindAnyDbContextWithDbSetForTypeException(Type type) : base($"Could not find a DbContext (that has been added to Cloudy by AddContext()) that has a DbSet with a type compatible with (or assignable to) {type}") { }
    }
}