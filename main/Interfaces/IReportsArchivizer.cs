using SNIClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuplaNotificationIntegration.Interfaces
{
    public interface IReportsArchivizer
    {
        Task ArchivizeReports(List<QuarterlyReport> quarterlyReports);
    }
}
