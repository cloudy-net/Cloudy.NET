using Microsoft.EntityFrameworkCore;
using System;

namespace Cloudy.NET.ContextSupport
{
    public interface IContextWrapper
    {
        DbContext Context { get; }
        object GetDbSet(Type type);
    }
}