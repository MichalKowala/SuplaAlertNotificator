using SNIClassLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuplaNotificationIntegration.Interfaces
{
    public interface IAlertsLogger
    {
        Task LogTheAlerts(List<QuarterlyReport> reports);
    }
}
