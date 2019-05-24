using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ContentControllerSupport
{
    public interface IContentControllerMatchCreator
    {
        IContentControllerMatch Create(string name, MethodInfo method);
    }
}
