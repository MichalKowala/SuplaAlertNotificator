using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SNIClassLibrary;
using SuplaNotificationIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;


[assembly: FunctionsStartup(typeof(SuplaNotificationIntegration.Startup))]
namespace SuplaNotificationIntegration
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IAlertsLogger, AlertsLogger>();
            builder.Services.AddTransient<IReportsArchivizer, ReportsArchivizer>();
            builder.Services.AddTransient<IReportsManager, ReportsManager>();
            builder.Services.AddTransient<IStorageAccessHelper, StorageAccessHelper>();
        }
    }
}
