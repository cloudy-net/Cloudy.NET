using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.DemoSupport
{
    public interface IDemoPageRenderer
    {
        Task RenderAsync(HttpContext context);
    }
}
