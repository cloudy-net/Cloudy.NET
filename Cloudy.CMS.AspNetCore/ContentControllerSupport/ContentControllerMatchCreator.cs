using Cloudy.CMS.ContentControllerSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.AspNetCore.ContentControllerSupport
{
    public class ContentControllerMatchCreator : IContentControllerMatchCreator
    {
        public IContentControllerMatch Create(string name, MethodInfo method)
        {
            return new ContentControllerMatch(name, method.Name);
        }
    }
}
