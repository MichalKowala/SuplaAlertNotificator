using System;
using System.Collections.Generic;
using System.Text;

namespace SuplaNotificationIntegration.Interfaces
{
    public interface IAlertsLogger
    {
        void LogTheAlerts(string message);
    }
}
