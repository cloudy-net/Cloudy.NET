using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.AspNetCore.Loggers
{
    public class DefaultLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            
        }
    }
}
