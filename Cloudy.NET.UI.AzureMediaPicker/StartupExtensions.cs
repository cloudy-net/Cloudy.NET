using Cloudy.CMS;
using Cloudy.CMS.UI.AzureMediaPicker;
using Cloudy.CMS.UI.FieldSupport.MediaPicker;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StartupExtensions
{
    public static CloudyConfigurator AddAzureMediaPicker(this CloudyConfigurator configurator)
    {
        configurator.Services.AddSingleton<IMediaProvider, AzureMediaProvider>();

        return configurator;
    }
}