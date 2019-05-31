using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContainerAttribute : Attribute
    {
        public string Id { get; }

        public ContainerAttribute(string id)
        {
            Id = id;
        }
    }
}
