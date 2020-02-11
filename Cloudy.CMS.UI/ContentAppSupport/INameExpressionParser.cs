using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    public interface INameExpressionParser
    {
        string Parse(Type type);
    }
}
