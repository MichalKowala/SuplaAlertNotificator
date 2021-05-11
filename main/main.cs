using Microsoft.Azure.WebJobs;
using SNIClassLibrary;
using SuplaNotificationIntegration.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuplaNotificationIntegration
{
    public class Function
    {
        private readonly IReportsManager _reportsManager;
        private readonly IReportsArchivizer _reportsArchivizer;
        private readonly IAlertsLogger _alertsLogger;
        public Function(IReportsManager reportsManager, IReportsArchivizer reportsArchivizer, IAlertsLogger alertsLogger)
        {
            _reportsManager = reportsManager;
            _reportsArchivizer = reportsArchivizer;
            _alertsLogger = alertsLogger;
        }
        [FunctionName("Executor")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            List<QuarterlyReport> reports = _reportsManager.GenerateQuarterlyReports();
            await _reportsArchivizer.ArchivizeTheReports(reports);
            await _alertsLogger.LogTheAlerts(reports);
            await _reportsManager.MailTheReports(reports);
        }
    }
}


