using SNIClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuplaNotificationIntegration.Interfaces
{
    public interface IReportsManager
    {
        List<QuarterlyReport> GenerateQuarterlyReports();
        Task MailTheReports(List<QuarterlyReport> reports);
        Task SMSTheReports(List<QuarterlyReport> reports);
    }
}
