using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    public interface IImageExpressionParser
    {
        string Parse(Type type);
    }
}
