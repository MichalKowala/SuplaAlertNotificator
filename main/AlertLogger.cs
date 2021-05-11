using Microsoft.Azure.Storage.Blob;
using SNIClassLibrary;
using SuplaNotificationIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SuplaNotificationIntegration
{
    public class AlertsLogger : IAlertsLogger
    {
        public async Task LogTheAlerts(List<QuarterlyReport> reports)
        {
            string fileName = EnvKeys.AlertLogsFolder + "/" + DateTime.Now.ToString("dd.MM.yyyy");
            StorageAccessHelper storage = new StorageAccessHelper();
            CloudBlockBlob blob = storage.GetSNIContainerBlockBlobReference(fileName);
            string message = "";
            foreach (QuarterlyReport report in reports)
            {
                foreach (string alert in report.IncorrectReadings)
                {
                    message += alert + Environment.NewLine + Environment.NewLine;
                }
            }

            using (MemoryStream stream = new MemoryStream())
            {
                if (blob.Exists())
                blob.DownloadToStream(stream);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(message);
                    sw.Flush();
                    stream.Position = 0;
                    blob.UploadFromStream(stream);
                }
            }
        }
    }
}
