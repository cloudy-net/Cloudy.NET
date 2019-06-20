using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.IO;

namespace Poetry.EmbeddedResourceSupport
{
    public class EmbeddedResource
    {
        public string AssemblyName { get; }
        public string Name { get; }

        public EmbeddedResource(string assemblyName, string name)
        {
            AssemblyName = assemblyName;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            var resource = obj as EmbeddedResource;

            if(resource == null)
            {
                return false;
            }

            return AssemblyName == resource.AssemblyName && Name == resource.Name;
        }

        public override int GetHashCode()
        {
            return $"{AssemblyName}:{Name}".GetHashCode();
        }
    }
}