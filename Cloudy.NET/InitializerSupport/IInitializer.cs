using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.InitializerSupport
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}
