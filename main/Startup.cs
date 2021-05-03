using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using SNIClassLibrary;
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
          
        }
    }
}
