using Microsoft.EntityFrameworkCore;
using System;

namespace Cloudy.CMS.ContextSupport
{
    public interface IContextWrapper : IDisposable
    {
        DbContext Context { get; }
        object GetDbSet(Type type);
    }
}