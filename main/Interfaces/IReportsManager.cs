using SNIClassLibrary;
using System.Collections.Generic;
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
