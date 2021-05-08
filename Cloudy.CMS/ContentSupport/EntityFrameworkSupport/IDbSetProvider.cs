using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public interface IDbSetProvider
    {
        IDbSetWrapper Get(string contentTypeId);
        IDbSetWrapper Get<T>() where T : class;
    }
}
