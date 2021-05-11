using SNIClassLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuplaNotificationIntegration.Interfaces
{
    public interface IReportsArchivizer
    {
        Task ArchivizeTheReports(List<QuarterlyReport> quarterlyReports);
    }
}
