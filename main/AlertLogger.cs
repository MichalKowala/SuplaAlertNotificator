using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using SuplaNotificationIntegration.Interfaces;
using System;
using System.IO;

namespace SuplaNotificationIntegration
{
    public class AlertsLogger : IAlertsLogger
    {
        public void LogTheAlerts(string message)
        {
            string fileName = EnvKeys.AlertLogsFolder + "/" + DateTime.Now.ToString("dd.MM.yyyy");
            StorageAccessHelper storage = new StorageAccessHelper();
            CloudBlockBlob blob = storage.GetSNIContainerBlockBlobReference(fileName);
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
